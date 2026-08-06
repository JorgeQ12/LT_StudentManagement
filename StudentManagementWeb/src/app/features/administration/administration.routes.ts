import { Routes } from '@angular/router';

export const ADMINISTRATION_ROUTES: Routes = [
  {
    path: '',
    children: [
      {
        path: '',
        pathMatch: 'full',
        title: 'Administración | Portal Académico',
        loadComponent: () =>
          import('./pages/dashboard/admin-dashboard-page').then(
            (component) => component.AdminDashboardPage,
          ),
      },
      {
        path: 'students',
        loadChildren: () =>
          import('@features/students').then((routes) => routes.STUDENTS_ADMIN_ROUTES),
      },
      {
        path: 'academic-programs',
        loadChildren: () =>
          import('@features/academic-programs').then(
            (routes) => routes.ACADEMIC_PROGRAMS_ADMIN_ROUTES,
          ),
      },
      {
        path: 'courses',
        loadChildren: () =>
          import('@features/courses').then((routes) => routes.COURSES_ADMIN_ROUTES),
      },
      {
        path: 'professors',
        loadChildren: () =>
          import('@features/professors').then((routes) => routes.PROFESSORS_ADMIN_ROUTES),
      },
    ],
  },
];
