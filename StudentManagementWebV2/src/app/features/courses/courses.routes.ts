import { Routes } from '@angular/router';
import { ProfessorsApiService } from '../professors/public-api';
import { CoursesApiService } from './data-access/courses-api.service';
import { CoursesFacade } from './facade/courses.facade';
export const COURSES_ROUTES: Routes = [
  {
    path: '',
    providers: [CoursesApiService, ProfessorsApiService, CoursesFacade],
    loadComponent: () =>
      import('./pages/course-list/course-list.page').then((module) => module.CourseListPage),
    children: [
      {
        path: 'new',
        loadComponent: () =>
          import('./components/course-form/course-form.component').then(
            (module) => module.CourseFormComponent,
          ),
      },
      {
        path: ':id/edit',
        loadComponent: () =>
          import('./components/course-form/course-form.component').then(
            (module) => module.CourseFormComponent,
          ),
      },
    ],
  },
];
