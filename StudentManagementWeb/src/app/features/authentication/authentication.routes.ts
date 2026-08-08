import { Routes } from '@angular/router';
import { guestGuard } from '../../core/auth/auth.guards';

export const AUTHENTICATION_ROUTES: Routes = [
  {
    path: '',
    canActivate: [guestGuard],
    loadComponent: () =>
      import('./pages/auth-layout/auth-layout.page').then((module) => module.AuthLayoutPage),
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'login' },
      {
        path: 'login',
        loadComponent: () => import('./pages/login/login.page').then((module) => module.LoginPage),
      },
      {
        path: 'register',
        loadComponent: () =>
          import('./pages/register/register.page').then((module) => module.RegisterPage),
      },
    ],
  },
];
