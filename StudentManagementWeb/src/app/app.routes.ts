import { Routes } from '@angular/router';
import { authenticatedGuard, homeRedirectGuard, roleGuard } from './core/auth/auth.guards';
import { AppShellComponent } from './layout/app-shell/app-shell.component';
import { RoutePendingComponent } from './shared/components/route-pending/route-pending.component';

export const routes: Routes = [
  {
    path: 'auth',
    loadChildren: () =>
      import('./features/authentication/authentication.routes').then(
        (module) => module.AUTHENTICATION_ROUTES,
      ),
  },
  {
    path: 'forbidden',
    loadComponent: () =>
      import('./shared/components/error-page/error-page.component').then(
        (module) => module.ErrorPageComponent,
      ),
    data: {
      code: '403',
      title: 'Acceso restringido',
      description: 'Tu cuenta no tiene permiso para ingresar a esta sección.',
    },
  },
  {
    path: '',
    component: AppShellComponent,
    canActivate: [authenticatedGuard],
    children: [
      {
        path: '',
        pathMatch: 'full',
        component: RoutePendingComponent,
        canActivate: [homeRedirectGuard],
      },
      {
        path: 'admin',
        canMatch: [roleGuard('Administrator')],
        loadChildren: () =>
          import('./features/administration/administration.routes').then(
            (module) => module.ADMINISTRATION_ROUTES,
          ),
      },
      {
        path: 'student',
        canMatch: [roleGuard('Student')],
        loadChildren: () =>
          import('./features/student-profile/student.routes').then(
            (module) => module.STUDENT_ROUTES,
          ),
      },
    ],
  },
  {
    path: '**',
    loadComponent: () =>
      import('./shared/components/error-page/error-page.component').then(
        (module) => module.ErrorPageComponent,
      ),
    data: {
      code: '404',
      title: 'Página no encontrada',
      description: 'La dirección solicitada no existe o cambió.',
    },
  },
];
