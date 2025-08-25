import { Component, inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

import { ViewEncapsulation } from '@angular/core';

@Component({
  standalone: true,
  selector: 'app-pago-dialog',
  imports: [CommonModule, MatDialogModule, MatButtonModule, ReactiveFormsModule, MatFormFieldModule, MatInputModule],
  template: `
    <div class="pago-modal">
      <h2 mat-dialog-title>Registrar pago</h2>
      <form [formGroup]="form" style="display: flex; flex-direction: column; gap: 18px; margin-top: 12px;">
        <mat-form-field appearance="fill">
          <mat-label>Monto del pago</mat-label>
          <input matInput type="number" formControlName="montoPago" step="0.01">
          <mat-error *ngIf="form.get('montoPago')?.hasError('min')">Debe ser mayor que 0</mat-error>
        </mat-form-field>
        <mat-form-field appearance="fill">
          <mat-label>Método de pago</mat-label>
          <input matInput formControlName="metodoPago" placeholder="Efectivo, Transferencia, ...">
        </mat-form-field>
        <div style="display: flex; gap: 18px; justify-content: flex-end;">
          <button mat-button (click)="ref.close(null)">Cancelar</button>
          <button mat-raised-button color="primary" [disabled]="form.invalid" (click)="registrar()">Registrar</button>
        </div>
      </form>
    </div>
  `,
  styleUrls: ['./pago-dialog.component.css']
})
export class PagoDialogComponent {
  ref = inject(MatDialogRef<PagoDialogComponent>);
  data = inject(MAT_DIALOG_DATA) as { deudaId: number };
  form: FormGroup;

  constructor() {
    const fb = inject(FormBuilder);
    this.form = fb.group({
      montoPago: [0, [Validators.required, Validators.min(0.01)]],
      metodoPago: ['', [Validators.required]]
    });
  }

  registrar() {
    if (this.form.invalid) return;
    this.ref.close({
      deudaId: this.data.deudaId,
      montoPago: this.form.value.montoPago,
      metodoPago: this.form.value.metodoPago
    });
  }
}
