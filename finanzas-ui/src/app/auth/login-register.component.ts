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
    email: ['', [Validators.required, Validators.email]],
    passwordHash: ['', [Validators.required, Validators.minLength(6)]],
  });

  doLogin() {
    if (this.loginForm.invalid) return;
  const email = this.loginForm.value.email ?? '';
  const passwordHash = this.loginForm.value.passwordHash ?? '';
  this.auth.login(email, passwordHash).subscribe((res: any) => {
      console.log('Respuesta login backend:', res); // <-- LOG para depuración
      // Soporta ambos formatos: res.usuario o res directo
      const usuario = res.usuario ?? res;
      this.auth.setUser({ usuarioId: usuario.usuarioId, nombre: usuario.nombre, email: usuario.email });
      this.router.navigate(['/deudas']);
    });
  }

  registerForm = this.fb.group({
    nombre: ['', [Validators.required, Validators.minLength(2)]],
    email: ['', [Validators.required, Validators.email]],
    passwordHash: ['', [Validators.required, Validators.minLength(6)]],
  });

  doRegister() {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched(); // Fuerza mostrar errores
      return;
    }
    this.auth.register(this.registerForm.value as any).subscribe((res: any) => {
      console.log('Respuesta registro backend:', res); // <-- LOG para depuración
      // Guardar usuario en el servicio tras registro
      this.auth.setUser({ usuarioId: res.usuarioId, nombre: res.nombre, email: res.email });
      this.router.navigate(['/deudas']);
    });
  }
}