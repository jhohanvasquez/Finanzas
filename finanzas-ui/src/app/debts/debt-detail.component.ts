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
import { ApiService, Deuda } from '../services/api.service';
import { ConfirmDialogComponent } from '../shared/confirm-dialog.component';

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
      // No hay endpoint para consultar una sola deuda, se puede obtener de la lista si es necesario
      // Aquí se podría hacer una consulta por usuario y filtrar por id
      this.api.getDeudasPorUsuario(1).subscribe(deudas => {
        const d = deudas.find(x => x.deudaId === id) || null;
        this.deuda.set(d);
        if (d) this.form.patchValue({ descripcion: d.descripcion, montoTotal: d.montoTotal });
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