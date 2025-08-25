import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatTabsModule } from '@angular/material/tabs';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { AuthService } from '../services/auth.service';

@Component({
  standalone: true,
  selector: 'app-login-register',
  templateUrl: './login-register.component.html',
  styleUrls: ['./login-register.component.css'],
  imports: [CommonModule, ReactiveFormsModule, MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatTabsModule, MatIconModule]
})
export class LoginRegisterComponent {
  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  private router = inject(Router);

  hide = true;

  loginForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]]
  });

  registerForm = this.fb.group({
    nombre: ['', [Validators.required, Validators.minLength(2)]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
  });

  doLogin() {
    if (this.loginForm.invalid) return;
    const { email } = this.loginForm.value;
    this.auth.loginMock(email!);
    this.router.navigate(['/deudas']);
  }

  doRegister() {
    if (this.registerForm.invalid) return;
    this.auth.register(this.registerForm.value as any).subscribe(() => {
      // Auto-login
      this.auth.loginMock(this.registerForm.value.email!);
      this.router.navigate(['/deudas']);
    });
  }
}