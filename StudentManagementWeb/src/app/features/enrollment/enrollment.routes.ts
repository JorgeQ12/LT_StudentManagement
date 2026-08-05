import { Routes } from '@angular/router';

import { EnrollmentFacade } from './facades/enrollment.facade';

export const ENROLLMENT_ROUTES: Routes = [
  {
    path: '',
    providers: [EnrollmentFacade],
    children: [
      {
        path: '',
        pathMatch: 'full',
        title: 'Mi inscripción | Portal Académico',
        loadComponent: () =>
          import('./pages/enrollment/enrollment-page').then(
            (component) => component.EnrollmentPage,
          ),
      },
      {
        path: 'classmates',
        title: 'Mis compañeros | Portal Académico',
        loadComponent: () =>
          import('./pages/classmates/classmates-page').then(
            (component) => component.ClassmatesPage,
          ),
      },
    ],
  },
];
