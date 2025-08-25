import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ApiService, Deuda } from '../../services/api.service';
import { AuthService } from '../../services/auth.service';
import { saveAs } from 'file-saver';

@Component({
  standalone: true,
  selector: 'app-debts-list',
  imports: [CommonModule, RouterLink, MatCardModule, MatButtonModule, MatIconModule, MatChipsModule, MatSelectModule, MatTableModule, MatProgressSpinnerModule],
  templateUrl: './debts-list.component.html',
  styleUrls: ['./debts-list.component.css']
})
export class DebtsListComponent implements OnInit {
  private api = inject(ApiService);
  private auth = inject(AuthService);

  loading = signal(true);
  filtro = signal<'todas' | 'pendientes' | 'pagadas'>('todas');
  deudas = signal<Deuda[]>([]);

  displayedColumns = ['id','descripcion','monto','estado','acciones'];

  ngOnInit() {
    this.cargar();
  }

  cargar() {
    this.loading.set(true);
    const user = this.auth.user();
    if (!user) {
      this.loading.set(false);
      return;
    }
    this.api.getDeudasPorUsuario(user.usuarioId).subscribe({
      next: (dataRaw: any[]) => {
        console.log('Deudas recibidas (raw):', dataRaw); // LOG para depuración
        if (!Array.isArray(dataRaw) || dataRaw.length === 0) {
          console.warn('La respuesta de la API no es un array o está vacía:', dataRaw);
          this.deudas.set([]);
          this.loading.set(false);
          return;
        }
        // Si el primer elemento es un string, mostrarlo
        if (typeof dataRaw[0] === 'string') {
          console.warn('La respuesta de la API es un string, no un array de arrays:', dataRaw);
        }
        // Si el primer elemento es un array, mostrarlo
        if (Array.isArray(dataRaw[0])) {
          console.log('Primer elemento del array:', dataRaw[0]);
        }
        // Transformar cada fila en objeto
        // La respuesta es un array de arrays tipo [[Nombre, juan topo], [DeudaId, 14], ...]
        // Si la respuesta es un solo array de pares clave-valor, conviértelo en un objeto Deuda
        let deudas: Deuda[] = [];
        if (Array.isArray(dataRaw)) {
          // Si es un array de objetos tipo {Nombre, DeudaId, ...}
          dataRaw.forEach((obj: any) => {
            if (obj && typeof obj === 'object' && 'DeudaId' in obj) {
              deudas.push({
                deudaId: Number((obj as any).DeudaId),
                usuarioId: user.usuarioId,
                descripcion: String((obj as any).Nombre),
                montoTotal: Number((obj as any).MontoTotal),
                estado: String((obj as any).Estado)
              });
            }
          });
        } else if (dataRaw && typeof dataRaw === 'object' && 'DeudaId' in dataRaw) {
          // Si es un solo objeto
          const obj = dataRaw as any;
          deudas = [{
            deudaId: Number(obj.DeudaId),
            usuarioId: user.usuarioId,
            descripcion: String(obj.Nombre),
            montoTotal: Number(obj.MontoTotal),
            estado: String(obj.Estado)
          }];
        }
        console.log('Deudas transformadas:', deudas);
        this.deudas.set(deudas);
        this.loading.set(false);
      },
      error: (err) => {
        console.error('Error al cargar deudas:', err); // LOG de error
        this.loading.set(false);
      }
    });
  }

  deudasFiltradas = computed(() => {
    const f = this.filtro();
    return this.deudas().filter(d => {
      if (f === 'todas') return true;
      if (f === 'pendientes') return d.estado === 'Pendiente';
      return d.estado === 'Pagada';
    });
  });

  exportarDeudas() {
    const deudas = this.deudasFiltradas();
    const csv = [
      ['ID', 'Descripción', 'Monto Total', 'Estado'],
      ...deudas.map(d => [d.deudaId, d.descripcion, d.montoTotal, d.estado])
    ].map(row => row.join(',')).join('\n');
    const blob = new Blob([csv], { type: 'text/csv;charset=utf-8;' });
    saveAs(blob, 'deudas.csv');
  }
}
