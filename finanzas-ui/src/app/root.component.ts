import { Component, inject, effect } from '@angular/core';
import { RouterOutlet, Router } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { CommonModule } from '@angular/common';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, MatToolbarModule, MatButtonModule, MatIconModule, MatSnackBarModule],
  template: `
    <mat-toolbar color="primary" class="toolbar">
      <button mat-icon-button (click)="goHome()" aria-label="Inicio">
        <mat-icon>account_balance_wallet</mat-icon>
      </button>
      <span>Finanzas · Deudas</span>
      <span class="spacer"></span>
      <button *ngIf="!auth.isLoggedIn()" mat-button routerLink="/auth">Login / Registro</button>
      <button *ngIf="auth.isLoggedIn()" mat-button (click)="logout()">Salir</button>
    </mat-toolbar>
    <div class="container">
      <router-outlet></router-outlet>
    </div>
  `
})
export class RootComponent {
  private snack = inject(MatSnackBar);
  auth = inject(AuthService);
  private router = inject(Router);

  constructor() {
    // Redirigir si el usuario ya está logueado y está en la raíz
    effect(() => {
      if (this.auth.isLoggedIn() && this.router.url === '/') {
        this.router.navigate(['/deudas']);
      }
    });
  }

  logout() {
    this.auth.logout();
    this.snack.open('Sesión finalizada', 'OK', { duration: 2000 });
    this.router.navigate(['/']);
  }

  goHome() {
    if (this.auth.isLoggedIn()) this.router.navigate(['/deudas']);
    else this.router.navigate(['/']);
  }
}