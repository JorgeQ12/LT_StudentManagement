import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, switchMap, throwError } from 'rxjs';
import { AntiforgeryService } from './antiforgery.service';
import { SessionState } from './session-state.service';

const SAFE_METHODS = new Set(['GET', 'HEAD', 'OPTIONS']);

export const credentialsAndAntiforgeryInterceptor: HttpInterceptorFn = (request, next) => {
  const credentialRequest = request.clone({ withCredentials: true });
  if (SAFE_METHODS.has(request.method) || request.url.endsWith('/GenerateAntiforgeryToken')) {
    return next(credentialRequest);
  }

  return inject(AntiforgeryService)
    .getToken()
    .pipe(
      switchMap((token) =>
        next(
          credentialRequest.clone({
            setHeaders: { 'X-CSRF-TOKEN': token },
          }),
        ),
      ),
    );
};

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
