import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, switchMap, throwError } from 'rxjs';
import { ApiProblemDetails } from '../api/api.models';
import { AntiforgeryService } from './antiforgery.service';
import { SessionState } from './session-state.service';

const SAFE_METHODS = new Set(['GET', 'HEAD', 'OPTIONS']);
const ANTIFORGERY_ERROR_CODE = 'InvalidAntiforgeryToken';

export const credentialsAndAntiforgeryInterceptor: HttpInterceptorFn = (request, next) => {
  const credentialRequest = request.clone({ withCredentials: true });
  if (SAFE_METHODS.has(request.method) || request.url.endsWith('/GenerateAntiforgeryToken')) {
    return next(credentialRequest);
  }

  const antiforgery = inject(AntiforgeryService);
  const sendWithToken = () =>
    antiforgery
      .getToken()
      .pipe(
        switchMap((token) =>
          next(credentialRequest.clone({ setHeaders: { 'X-CSRF-TOKEN': token } })),
        ),
      );

  return sendWithToken().pipe(
    catchError((error: unknown) => {
      if (!isAntiforgeryError(error)) {
        return throwError(() => error);
      }
     
      antiforgery.clear();
      return sendWithToken();
    }),
  );
};

function isAntiforgeryError(error: unknown): boolean {
  return (
    error instanceof HttpErrorResponse &&
    error.status === 400 &&
    (error.error as ApiProblemDetails | null)?.code === ANTIFORGERY_ERROR_CODE
  );
}

export const sessionInterceptor: HttpInterceptorFn = (request, next) => {
  const session = inject(SessionState);
  const router = inject(Router);
  return next(request).pipe(
    catchError((error: unknown) => {
      if (
        error instanceof HttpErrorResponse &&
        error.status === 401 &&
        session.isAuthenticated() &&
        !request.url.endsWith('/Login')
      ) {
        session.clear();
        void router.navigate(['/auth/login'], { queryParams: { sessionExpired: '1' } });
      }
      return throwError(() => error);
    }),
  );
};
