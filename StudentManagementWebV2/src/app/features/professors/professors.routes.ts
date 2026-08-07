import { Routes } from '@angular/router';
import { ProfessorsApiService } from './data-access/professors-api.service';
import { ProfessorsFacade } from './facade/professors.facade';

export const PROFESSORS_ROUTES: Routes = [
  {
    path: '',
    providers: [ProfessorsApiService, ProfessorsFacade],
    loadComponent: () =>
      import('./pages/professor-list/professor-list.page').then(
        (module) => module.ProfessorListPage,
      ),
    children: [
      {
        path: 'new',
        loadComponent: () =>
          import('./components/professor-form/professor-form.component').then(
            (module) => module.ProfessorFormComponent,
          ),
      },
      {
        path: ':id/edit',
        loadComponent: () =>
          import('./components/professor-form/professor-form.component').then(
            (module) => module.ProfessorFormComponent,
          ),
      },
    ],
  },
];
