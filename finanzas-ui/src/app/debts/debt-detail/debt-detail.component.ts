import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ApiService, Deuda } from '../../services/api.service';
import { ConfirmDialogComponent } from '../../shared/confirm-dialog.component';

@Component({
  standalone: true,
  selector: 'app-debt-detail',
  templateUrl: './debt-detail.component.html',
  styleUrls: ['./debt-detail.component.css'],
  imports: [CommonModule, RouterLink, ReactiveFormsModule, MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatChipsModule, MatDialogModule]
})
export class DeudaDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private api = inject(ApiService);
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private dialog = inject(MatDialog);

  deuda = signal<Deuda | null>(null);

  form = this.fb.group({
    descripcion: ['', [Validators.required, Validators.minLength(3)]],
    montoTotal: [0, [Validators.required, Validators.min(0.01)]],
  });

  pagoForm = this.fb.group({
    montoPago: [0, [Validators.required, Validators.min(0.01)]],
    metodoPago: ['', [Validators.required]]
  });

  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.cargar(id);
  }

  cargar(id: number) {
      // Consulta todas las deudas y busca la que coincide con el id
      console.log('Iniciando carga de detalle para id:', id);
      this.api.getDeudasPorUsuario(1).subscribe({
        next: (deudas) => {
          alert('Respuesta de la API en detalle: ' + JSON.stringify(deudas));
          console.log('Respuesta de la API en detalle:', JSON.stringify(deudas));
          let deuda: Deuda | null = null;
          // Si la respuesta es un solo objeto, úsalo directamente
          if (deudas && typeof deudas === 'object' && !Array.isArray(deudas) && ('DeudaId' in deudas || 'deudaId' in deudas)) {
            const obj = deudas as any;
            alert('Deuda encontrada como objeto: ' + JSON.stringify(obj));
            console.log('Deuda encontrada como objeto:', JSON.stringify(obj));
            deuda = {
              deudaId: Number(obj.DeudaId ?? obj.deudaId),
              usuarioId: obj.usuarioId ?? 1,
              descripcion: String(obj.Nombre ?? obj.descripcion ?? ''),
              montoTotal: Number(obj.MontoTotal ?? obj.montoTotal ?? 0),
              estado: String(obj.Estado ?? obj.estado ?? '')
            };
          } else if (Array.isArray(deudas)) {
            // Si es array de objetos tipo {DeudaId, ...}
            const encontrada = (deudas as any[]).find(x => Number(x.deudaId ?? x.DeudaId) === id);
            console.log('Deuda encontrada en array:', JSON.stringify(encontrada));
            if (encontrada) {
              deuda = {
                deudaId: Number(encontrada.DeudaId ?? encontrada.deudaId),
                usuarioId: encontrada.usuarioId ?? 1,
                descripcion: String(encontrada.Nombre ?? encontrada.descripcion ?? ''),
                montoTotal: Number(encontrada.MontoTotal ?? encontrada.montoTotal ?? 0),
                estado: String(encontrada.Estado ?? encontrada.estado ?? '')
              };
            }
          }
          alert('Deuda final para mostrar: ' + JSON.stringify(deuda));
          console.log('Deuda final para mostrar:', JSON.stringify(deuda));
          this.deuda.set(deuda);
          if (deuda) this.form.patchValue({ descripcion: deuda.descripcion, montoTotal: deuda.montoTotal });
        },
        error: (err) => {
          alert('Error al consultar la deuda: ' + JSON.stringify(err));
          console.error('Error al consultar la deuda:', err);
        }
      });
  }

  guardarCambios() {
    const d = this.deuda();
    if (!d) return;
    if (d.estado === 'Pagada') { alert('Una deuda pagada no puede ser modificada.'); return; }
    if (this.form.invalid) return;
    const dto = { ...d, ...this.form.value };
    // No hay endpoint para editar deuda, solo para registrar
    // Se podría implementar si el backend lo soporta
  }

  eliminar() {
    const d = this.deuda();
    if (!d) return;
    if (d.estado === 'Pagada') { alert('Una deuda pagada no puede ser eliminada.'); return; }
    alert('Funcionalidad de eliminar deuda no implementada en el backend.');
  }

  registrarPago() {
    const d = this.deuda();
    if (!d) return;
    if (this.pagoForm.invalid) return;
    const dto = { deudaId: d.deudaId!, montoPago: this.pagoForm.value.montoPago!, metodoPago: this.pagoForm.value.metodoPago! };
    this.api.registrarPago(dto).subscribe(() => this.cargar(d.deudaId!));
  }
}
