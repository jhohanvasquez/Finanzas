import { Component, inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  selector: 'app-confirm-dialog',
  imports: [CommonModule, MatDialogModule, MatButtonModule],
  template: `
    <h2 mat-dialog-title>{{data.title}}</h2>
    <div mat-dialog-content>{{data.message}}</div>
    <div mat-dialog-actions>
      <button mat-button (click)="ref.close(false)">Cancelar</button>
      <button mat-raised-button color="warn" (click)="ref.close(true)">Confirmar</button>
    </div>
  `
})
export class ConfirmDialogComponent {
  ref = inject(MatDialogRef<ConfirmDialogComponent>);
  data = inject<{ title: string; message: string }>(MAT_DIALOG_DATA);
}