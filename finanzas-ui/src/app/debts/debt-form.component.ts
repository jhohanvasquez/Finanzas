import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { ApiService } from '../services/api.service';
import { AuthService } from '../services/auth.service';

@Component({
  standalone: true,
  selector: 'app-debt-form',
  templateUrl: './debt-form.component.html',
  styleUrls: ['./debt-form.component.css'],
  imports: [CommonModule, ReactiveFormsModule, MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule]
})
export class DeudaFormComponent {
  private fb = inject(FormBuilder);
  private api = inject(ApiService);
  private auth = inject(AuthService);
  private router = inject(Router);

  form = this.fb.group({
    montoTotal: [0, [Validators.required, Validators.min(0.01)]],
    descripcion: ['', [Validators.required, Validators.minLength(3)]],
  });

  guardar() {
    if (this.form.invalid) return;
    const user = this.auth.user();
    if (!user) return;
    const dto = {
      usuarioId: user.usuarioId,
      montoTotal: this.form.value.montoTotal!,
      descripcion: this.form.value.descripcion!,
    };
    this.api.crearDeuda(dto).subscribe(() => this.router.navigate(['/deudas']));
  }
}