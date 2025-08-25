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
    if (!user) return;
    this.api.getDeudasPorUsuario(user.usuarioId).subscribe({
      next: (data) => { this.deudas.set(data); this.loading.set(false); },
      error: () => this.loading.set(false)
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
}
