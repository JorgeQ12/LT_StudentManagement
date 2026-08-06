import { Routes } from '@angular/router';

import { pendingChangesGuard } from '@core/navigation/pending-changes.guard';

import { StudentsFacade } from './facades/students.facade';

export const STUDENTS_ADMIN_ROUTES: Routes = [
  {
    path: '',
    providers: [StudentsFacade],
    children: [
      {
        path: '',
        pathMatch: 'full',
        title: 'Estudiantes | Portal Académico',
        loadComponent: () =>
          import('./pages/students-list/students-list-page').then(
            (component) => component.StudentsListPage,
          ),
      },
      {
        path: 'new',
        canDeactivate: [pendingChangesGuard],
        title: 'Crear estudiante | Portal Académico',
        loadComponent: () =>
          import('./pages/student-form/student-form-page').then(
            (component) => component.StudentFormPage,
          ),
      },
      {
        path: ':id/edit',
        canDeactivate: [pendingChangesGuard],
        title: 'Editar estudiante | Portal Académico',
        loadComponent: () =>
          import('./pages/student-form/student-form-page').then(
            (component) => component.StudentFormPage,
          ),
      },
    ],
  },
];
