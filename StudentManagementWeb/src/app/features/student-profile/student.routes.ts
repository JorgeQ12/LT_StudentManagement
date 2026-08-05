import { Routes } from '@angular/router';

import { pendingChangesGuard } from '@core/navigation/pending-changes.guard';
import { StudentWorkspaceFacade } from './facades/student-workspace.facade';

export const STUDENT_ROUTES: Routes = [
  {
    path: '',
    providers: [StudentWorkspaceFacade],
    children: [
      {
        path: '',
        pathMatch: 'full',
        title: 'Inicio | Portal Académico',
        loadComponent: () =>
          import('./pages/dashboard/student-dashboard-page').then(
            (component) => component.StudentDashboardPage,
          ),
      },
      {
        path: 'profile',
        canDeactivate: [pendingChangesGuard],
        title: 'Mi perfil | Portal Académico',
        loadComponent: () =>
          import('./pages/profile/student-profile-page').then(
            (component) => component.StudentProfilePage,
          ),
      },
      {
        path: 'enrollment',
        loadChildren: () =>
          import('@features/enrollment').then((routes) => routes.ENROLLMENT_ROUTES),
      },
    ],
  },
];
