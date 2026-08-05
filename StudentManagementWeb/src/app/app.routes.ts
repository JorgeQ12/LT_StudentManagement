import { Routes } from '@angular/router';

import { entryRedirectGuard, guestGuard, roleGuard } from '@core/auth/auth.guards';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    canActivate: [entryRedirectGuard],
    loadComponent: () =>
      import('./shared/ui/route-loading-page').then((component) => component.RouteLoadingPage),
  },
  {
    path: '',
    loadComponent: () =>
      import('./features/authentication/pages/auth-layout/auth-layout').then(
        (component) => component.AuthLayout,
      ),
    children: [
      {
        path: 'login',
        canMatch: [guestGuard],
        title: 'Iniciar sesión | Portal Académico',
        loadComponent: () =>
          import('./features/authentication/pages/login/login-page').then(
            (component) => component.LoginPage,
          ),
      },
      {
        path: 'register',
        canMatch: [guestGuard],
        title: 'Crear cuenta | Portal Académico',
        loadComponent: () =>
          import('./features/authentication/pages/register/register-page').then(
            (component) => component.RegisterPage,
          ),
      },
    ],
  },
  {
    path: 'student',
    canMatch: [roleGuard('Student')],
    loadComponent: () => import('./core/layout/app-shell').then((component) => component.AppShell),
    loadChildren: () =>
      import('./features/student-profile/student.routes').then((routes) => routes.STUDENT_ROUTES),
  },
  {
    path: 'admin',
    canMatch: [roleGuard('Administrator')],
    loadComponent: () => import('./core/layout/app-shell').then((component) => component.AppShell),
    loadChildren: () =>
      import('./features/administration/administration.routes').then(
        (routes) => routes.ADMINISTRATION_ROUTES,
      ),
  },
  {
    path: 'forbidden',
    title: 'Acceso restringido | Portal Académico',
    loadComponent: () => import('./shared/ui/error-page').then((component) => component.ErrorPage),
    data: {
      status: '403',
      title: 'No tienes acceso a esta sección',
      description: 'Tu perfil no cuenta con los permisos necesarios para abrir este contenido.',
    },
  },
  {
    path: '**',
    title: 'Página no encontrada | Portal Académico',
    loadComponent: () => import('./shared/ui/error-page').then((component) => component.ErrorPage),
    data: {
      status: '404',
      title: 'Esta página no existe',
      description: 'Es posible que el enlace haya cambiado o que la dirección esté incompleta.',
    },
  },
];
