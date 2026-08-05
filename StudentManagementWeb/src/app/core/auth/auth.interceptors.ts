import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, switchMap, throwError } from 'rxjs';

import { AntiforgeryService } from './antiforgery.service';
import { SessionState } from './session-state';

const UNSAFE_METHODS = new Set(['POST', 'PUT', 'PATCH', 'DELETE']);

export const credentialsInterceptor: HttpInterceptorFn = (request, next) =>
  next(request.clone({ withCredentials: true }));

export const antiforgeryInterceptor: HttpInterceptorFn = (request, next) => {
  if (!UNSAFE_METHODS.has(request.method.toUpperCase())) {
    return next(request);
  }

  const antiforgery = inject(AntiforgeryService);
  return antiforgery.ensureToken().pipe(
    switchMap((token) =>
      next(
        request.clone({
          setHeaders: { 'X-CSRF-TOKEN': token },
        }),
      ),
    ),
  );
};

export const sessionInterceptor: HttpInterceptorFn = (request, next) => {
  const sessionState = inject(SessionState);
  const router = inject(Router);

  return next(request).pipe(
    catchError((error: unknown) => {
      if (
        error instanceof HttpErrorResponse &&
        error.status === 401 &&
        sessionState.isAuthenticated()
      ) {
        sessionState.clear();
        void router.navigate(['/login'], { queryParams: { reason: 'expired' } });
      }

      return throwError(() => error);
    }),
  );
};
