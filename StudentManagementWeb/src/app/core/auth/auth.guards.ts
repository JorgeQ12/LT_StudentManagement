import { inject } from '@angular/core';
import { CanActivateFn, CanMatchFn, Router } from '@angular/router';

import { AccountRole } from '@core/api/api.models';

import { AuthFacade } from './auth.facade';

export const entryRedirectGuard: CanActivateFn = async () => {
  const authFacade = inject(AuthFacade);
  const router = inject(Router);
  await authFacade.initialize();
  const user = authFacade.user();
  return user ? router.parseUrl(authFacade.homeFor(user.role)) : router.parseUrl('/login');
};

export const guestGuard: CanMatchFn = async () => {
  const authFacade = inject(AuthFacade);
  const router = inject(Router);
  await authFacade.initialize();
  const user = authFacade.user();
  return user ? router.parseUrl(authFacade.homeFor(user.role)) : true;
};

export const roleGuard =
  (role: AccountRole): CanMatchFn =>
  async () => {
    const authFacade = inject(AuthFacade);
    const router = inject(Router);
    await authFacade.initialize();
    const user = authFacade.user();

    if (!user) {
      return router.parseUrl('/login');
    }

    return user.role === role ? true : router.parseUrl('/forbidden');
  };
