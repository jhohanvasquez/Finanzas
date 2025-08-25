import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

export interface Deuda {
  deudaId?: number;
  usuarioId: number;
  montoTotal: number;
  descripcion: string;
  pagada?: boolean;
  saldoPendiente?: number;
  fechaCreacion?: string;
}

export interface Pago {
  pagoId?: number;
  deudaId: number;
  montoPago: number;
  metodoPago: string;
  fechaPago?: string;
}

@Injectable({ providedIn: 'root' })
export class ApiService {
  private base = environment.apiBaseUrl;

  constructor(private http: HttpClient) {}

  // Deudas
  getDeudas() { return this.http.get<Deuda[]>(`${this.base}/deudas`); }
  getDeudasPorUsuario(usuarioId: number) { return this.http.get<Deuda[]>(`${this.base}/deudas/usuario/${usuarioId}`); }
  getDeuda(id: number) { return this.http.get<Deuda>(`${this.base}/deudas/${id}`); }
  crearDeuda(dto: Deuda) { return this.http.post<Deuda>(`${this.base}/deudas`, dto); }
  editarDeuda(id: number, dto: Deuda) { return this.http.put<Deuda>(`${this.base}/deudas/${id}`, dto); }
  eliminarDeuda(id: number) { return this.http.delete<void>(`${this.base}/deudas/${id}`); }

  // Pagos
  registrarPago(dto: Pago) { return this.http.post<Pago>(`${this.base}/pagos`, dto); }
}