import { Routes } from '@angular/router';

import { pendingChangesGuard } from '@core/navigation/pending-changes.guard';

import { ProfessorsFacade } from './facades/professors.facade';

export const PROFESSORS_ADMIN_ROUTES: Routes = [
  {
    path: '',
    providers: [ProfessorsFacade],
    children: [
      {
        path: '',
        pathMatch: 'full',
        title: 'Profesores | Portal Académico',
        loadComponent: () =>
          import('./pages/professors-list/professors-list-page').then(
            (component) => component.ProfessorsListPage,
          ),
      },
      {
        path: 'new',
        canDeactivate: [pendingChangesGuard],
        title: 'Crear profesor | Portal Académico',
        loadComponent: () =>
          import('./pages/professor-form/professor-form-page').then(
            (component) => component.ProfessorFormPage,
          ),
      },
      {
        path: ':id/edit',
        canDeactivate: [pendingChangesGuard],
        title: 'Editar profesor | Portal Académico',
        loadComponent: () =>
          import('./pages/professor-form/professor-form-page').then(
            (component) => component.ProfessorFormPage,
          ),
      },
    ],
  },
];
