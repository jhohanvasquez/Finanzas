
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

export interface Deuda {
  deudaId?: number;
  usuarioId: number;
  montoTotal: number;
  descripcion: string;
  estado?: string;
}

export interface Pago {
  pagoId?: number;
  deudaId: number;
  montoPago: number;
  metodoPago: string;
}

@Injectable({ providedIn: 'root' })
export class ApiService {
  private base = environment.apiBaseUrl;

  constructor(private http: HttpClient) {}

  // Deudas
  getDeudasPorUsuario(usuarioId: number) {
    return this.http.get<Deuda[]>(`${this.base}/Deuda/consultar/${usuarioId}`);
  }

  crearDeuda(dto: Deuda) {
    // El endpoint espera deudaId=0 para registro
    const body = { ...dto, deudaId: 0, estado: 'Pendiente' };
    return this.http.post<Deuda>(`${this.base}/Deuda/registrar`, body);
  }

  // Pagos
  registrarPago(dto: Pago) {
    // El endpoint espera pagoId=0 para registro
    const body = { ...dto, pagoId: 0 };
    return this.http.post<Pago>(`${this.base}/Pago/registrar`, body);
  }
}