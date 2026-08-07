import { Routes } from '@angular/router';
import { AcademicProgramsApiService } from './data-access/academic-programs-api.service';
import { AcademicProgramsFacade } from './facade/academic-programs.facade';

export const ACADEMIC_PROGRAMS_ROUTES: Routes = [
  {
    path: '',
    providers: [AcademicProgramsApiService, AcademicProgramsFacade],
    loadComponent: () =>
      import('./pages/program-list/program-list.page').then((module) => module.ProgramListPage),
    children: [
      {
        path: 'new',
        loadComponent: () =>
          import('./components/program-form/program-form.component').then(
            (module) => module.ProgramFormComponent,
          ),
      },
      {
        path: ':id/edit',
        loadComponent: () =>
          import('./components/program-form/program-form.component').then(
            (module) => module.ProgramFormComponent,
          ),
      },
    ],
  },
];
