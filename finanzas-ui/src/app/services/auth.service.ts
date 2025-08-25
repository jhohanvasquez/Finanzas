
import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

export interface RegisterDto {
  usuarioId: number;
  nombre: string;
  email: string;
  passwordHash: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly KEY = 'finanzas_user';
  user = signal<{ usuarioId: number; nombre: string; email: string } | null>(null);

  constructor(private http: HttpClient) {
    const stored = localStorage.getItem(this.KEY);
    if (stored) this.user.set(JSON.parse(stored));
  }

  isLoggedIn() { return !!this.user(); }

  register(dto: RegisterDto) {
    // El endpoint espera usuarioId=0 para registro
    const body = { ...dto, usuarioId: 0 };
    return this.http.post(environment.apiBaseUrl + '/Usuario/registrar', body);
  }

  setUser(user: { usuarioId: number; nombre: string; email: string }) {
    this.user.set(user);
    localStorage.setItem(this.KEY, JSON.stringify(user));
  }

  logout() {
    this.user.set(null);
    localStorage.removeItem(this.KEY);
  }
}