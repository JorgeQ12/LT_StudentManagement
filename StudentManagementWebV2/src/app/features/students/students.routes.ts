import { Routes } from '@angular/router';
import { StudentsApiService } from './data-access/students-api.service';
import { StudentsFacade } from './facade/students.facade';
export const STUDENTS_ROUTES: Routes = [
  {
    path: '',
    providers: [StudentsApiService, StudentsFacade],
    loadComponent: () =>
      import('./pages/student-list/student-list.page').then((module) => module.StudentListPage),
    children: [
      {
        path: 'new',
        loadComponent: () =>
          import('./components/student-form/student-form.component').then(
            (module) => module.StudentFormComponent,
          ),
      },
      {
        path: ':id/edit',
        loadComponent: () =>
          import('./components/student-form/student-form.component').then(
            (module) => module.StudentFormComponent,
          ),
      },
    ],
  },
];
