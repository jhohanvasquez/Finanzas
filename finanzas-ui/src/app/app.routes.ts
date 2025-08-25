import { Routes } from '@angular/router';
import { canActivateAuth } from './guards/auth.guard';

export const routes: Routes = [
  // Página inicial: Login / Registro directamente en la raíz
  { path: '', loadComponent: () => import('./auth/login-register.component').then(m => m.LoginRegisterComponent), pathMatch: 'full' },
  // Alias /auth para compatibilidad con enlaces existentes
  { path: 'auth', redirectTo: '', pathMatch: 'full' },
  { path: 'deudas', canActivate: [canActivateAuth], loadComponent: () => import('./debts/debts-list/debts-list.component').then(m => m.DebtsListComponent) },
  { path: 'deudas/nueva', canActivate: [canActivateAuth], loadComponent: () => import('./debts/debt-form/debt-form.component').then(m => m.DeudaFormComponent) },
  { path: 'deudas/:id', canActivate: [canActivateAuth], loadComponent: () => import('./debts/debt-detail/debt-detail.component').then(m => m.DeudaDetailComponent) },
  { path: '**', redirectTo: '' }
];