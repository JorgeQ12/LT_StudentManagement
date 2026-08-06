import { Routes } from '@angular/router';

import { pendingChangesGuard } from '@core/navigation/pending-changes.guard';

import { AcademicProgramsFacade } from './facades/academic-programs.facade';

export const ACADEMIC_PROGRAMS_ADMIN_ROUTES: Routes = [
  {
    path: '',
    providers: [AcademicProgramsFacade],
    children: [
      {
        path: '',
        pathMatch: 'full',
        title: 'Programas académicos | Portal Académico',
        loadComponent: () =>
          import('./pages/academic-programs-list/academic-programs-list-page').then(
            (component) => component.AcademicProgramsListPage,
          ),
      },
      {
        path: 'new',
        canDeactivate: [pendingChangesGuard],
        title: 'Crear programa | Portal Académico',
        loadComponent: () =>
          import('./pages/academic-program-form/academic-program-form-page').then(
            (component) => component.AcademicProgramFormPage,
          ),
      },
      {
        path: ':id/edit',
        canDeactivate: [pendingChangesGuard],
        title: 'Editar programa | Portal Académico',
        loadComponent: () =>
          import('./pages/academic-program-form/academic-program-form-page').then(
            (component) => component.AcademicProgramFormPage,
          ),
      },
    ],
  },
];
