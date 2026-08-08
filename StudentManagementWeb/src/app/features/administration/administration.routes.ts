import { Routes } from '@angular/router';
export const ADMINISTRATION_ROUTES: Routes = [
  {
    path: '',
    pathMatch: 'full',
    loadComponent: () =>
      import('./pages/dashboard/admin-dashboard.page').then((module) => module.AdminDashboardPage),
  },
  {
    path: 'programs',
    loadChildren: () =>
      import('../academic-programs/academic-programs.routes').then(
        (module) => module.ACADEMIC_PROGRAMS_ROUTES,
      ),
  },
  {
    path: 'courses',
    loadChildren: () => import('../courses/courses.routes').then((module) => module.COURSES_ROUTES),
  },
  {
    path: 'professors',
    loadChildren: () =>
      import('../professors/professors.routes').then((module) => module.PROFESSORS_ROUTES),
  },
  {
    path: 'students',
    loadChildren: () =>
      import('../students/students.routes').then((module) => module.STUDENTS_ROUTES),
  },
];
