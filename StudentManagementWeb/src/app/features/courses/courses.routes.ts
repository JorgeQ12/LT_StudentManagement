import { Routes } from '@angular/router';

import { pendingChangesGuard } from '@core/navigation/pending-changes.guard';

import { CoursesFacade } from './facades/courses.facade';

export const COURSES_ADMIN_ROUTES: Routes = [
  {
    path: '',
    providers: [CoursesFacade],
    children: [
      {
        path: '',
        pathMatch: 'full',
        title: 'Materias | Portal Académico',
        loadComponent: () =>
          import('./pages/courses-list/courses-list-page').then(
            (component) => component.CoursesListPage,
          ),
      },
      {
        path: 'new',
        canDeactivate: [pendingChangesGuard],
        title: 'Crear materia | Portal Académico',
        loadComponent: () =>
          import('./pages/course-form/course-form-page').then(
            (component) => component.CourseFormPage,
          ),
      },
      {
        path: ':id/edit',
        canDeactivate: [pendingChangesGuard],
        title: 'Editar materia | Portal Académico',
        loadComponent: () =>
          import('./pages/course-form/course-form-page').then(
            (component) => component.CourseFormPage,
          ),
      },
    ],
  },
];
