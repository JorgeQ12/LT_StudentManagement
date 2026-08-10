import { inject } from '@angular/core';
import { CanActivateFn, CanMatchFn, Router } from '@angular/router';
import { map } from 'rxjs';
import { AccountRole } from '../api/api.models';
import { AuthFacade } from './auth.facade';

export const authenticatedGuard: CanActivateFn = (_route, state) => {
  const auth = inject(AuthFacade);
  const router = inject(Router);
  return auth
    .ensureSession()
    .pipe(
      map((user) =>
        user
          ? true
          : router.createUrlTree(['/auth/login'], { queryParams: { returnUrl: state.url } }),
      ),
    );
};

export function roleGuard(role: AccountRole): CanMatchFn {
  return () => {
    const auth = inject(AuthFacade);
    const router = inject(Router);
    return auth
      .ensureSession()
      .pipe(map((user) => user?.role === role || router.createUrlTree(['/forbidden'])));
  };
}
