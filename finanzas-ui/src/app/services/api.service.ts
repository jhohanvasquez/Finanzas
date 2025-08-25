
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
  getDeudasPorUsuario(usuarioId: number): import('rxjs').Observable<Deuda[]> {
    return this.http.get<Deuda[]>(`${this.base}/Deuda/consultar/${usuarioId}`);
  }

  crearDeuda(dto: Omit<Deuda, 'deudaId' | 'estado'>): import('rxjs').Observable<Deuda> {
    const body: Deuda = { ...dto, deudaId: 0, estado: 'Pendiente' };
    return this.http.post<Deuda>(`${this.base}/Deuda/registrar`, body);
  }

  registrarPago(dto: Omit<Pago, 'pagoId'>): import('rxjs').Observable<Pago> {
    const body: Pago = { ...dto, pagoId: 0 };
    return this.http.post<Pago>(`${this.base}/Pago/registrar`, body);
  }
}