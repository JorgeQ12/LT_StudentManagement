import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiProblemDetails } from './api.models';

@Injectable({ providedIn: 'root' })
export class ApiErrorService {
  getProblem(error: unknown): ApiProblemDetails | null {
    if (!(error instanceof HttpErrorResponse) || !error.error || typeof error.error !== 'object') {
      return null;
    }

    return error.error as ApiProblemDetails;
  }

  getMessage(error: unknown): string {
    const problem = this.getProblem(error);
    if (problem?.detail) {
      return problem.detail;
    }

    if (error instanceof HttpErrorResponse) {
      if (error.status === 0) {
        return 'No fue posible conectar con el servicio. Revisa tu conexión e inténtalo de nuevo.';
      }

      const messages: Readonly<Record<number, string>> = {
        400: 'La solicitud contiene información inválida.',
        401: 'Tu sesión no es válida. Inicia sesión nuevamente.',
        403: 'No tienes permiso para realizar esta acción.',
        404: 'No se encontró el recurso solicitado.',
        409: 'La información cambió o entra en conflicto con otra operación.',
        422: 'La operación no cumple las reglas del sistema.',
        429: 'Se realizaron demasiados intentos. Espera un momento.',
        500: 'Ocurrió un error inesperado en el servicio.',
        503: 'El servicio no está disponible temporalmente.',
      };
      return messages[error.status] ?? 'No fue posible completar la operación.';
    }

    return 'No fue posible completar la operación.';
  }

  getFieldErrors(error: unknown): Readonly<Record<string, readonly string[]>> {
    return this.getProblem(error)?.errors ?? {};
  }
}
