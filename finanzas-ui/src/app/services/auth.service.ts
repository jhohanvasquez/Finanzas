import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

export interface RegisterDto {
  nombre: string;
  email: string;
  password: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly KEY = 'finanzas_user';
  user = signal<{ nombre: string; email: string } | null>(null);

  constructor(private http: HttpClient) {
    const stored = localStorage.getItem(this.KEY);
    if (stored) this.user.set(JSON.parse(stored));
  }

  isLoggedIn() { return !!this.user(); }

  loginMock(email: string) {
    // Como la API de ejemplo no define endpoint de login, hacemos un login local guardando el email.
    const u = { nombre: email.split('@')[0], email };
    this.user.set(u);
    localStorage.setItem(this.KEY, JSON.stringify(u));
  }

  register(dto: RegisterDto) {
    return this.http.post(environment.apiBaseUrl + '/usuarios', dto);
  }

  logout() {
    this.user.set(null);
    localStorage.removeItem(this.KEY);
  }
}