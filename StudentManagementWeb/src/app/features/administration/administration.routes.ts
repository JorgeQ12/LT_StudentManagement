import { Routes } from '@angular/router';

import { pendingChangesGuard } from '@core/navigation/pending-changes.guard';

import { AdministrationFacade } from './facades/administration.facade';

export const ADMINISTRATION_ROUTES: Routes = [
  {
    path: '',
    providers: [AdministrationFacade],
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
        pathMatch: 'full',
        title: 'Estudiantes | Portal Académico',
        loadComponent: () =>
          import('./pages/resource-list/admin-resource-list-page').then(
            (component) => component.AdminResourceListPage,
          ),
        data: { resource: 'students' },
      },
      {
        path: 'students/new',
        canDeactivate: [pendingChangesGuard],
        title: 'Crear estudiante | Portal Académico',
        loadComponent: () =>
          import('./pages/student-form/admin-student-form-page').then(
            (component) => component.AdminStudentFormPage,
          ),
      },
      {
        path: 'students/:id/edit',
        canDeactivate: [pendingChangesGuard],
        title: 'Editar estudiante | Portal Académico',
        loadComponent: () =>
          import('./pages/student-form/admin-student-form-page').then(
            (component) => component.AdminStudentFormPage,
          ),
      },
      {
        path: 'academic-programs',
        pathMatch: 'full',
        title: 'Programas académicos | Portal Académico',
        loadComponent: () =>
          import('./pages/resource-list/admin-resource-list-page').then(
            (component) => component.AdminResourceListPage,
          ),
        data: { resource: 'academic-programs' },
      },
      {
        path: 'academic-programs/new',
        canDeactivate: [pendingChangesGuard],
        title: 'Crear programa | Portal Académico',
        loadComponent: () =>
          import('./pages/program-form/admin-program-form-page').then(
            (component) => component.AdminProgramFormPage,
          ),
      },
      {
        path: 'academic-programs/:id/edit',
        canDeactivate: [pendingChangesGuard],
        title: 'Editar programa | Portal Académico',
        loadComponent: () =>
          import('./pages/program-form/admin-program-form-page').then(
            (component) => component.AdminProgramFormPage,
          ),
      },
      {
        path: 'courses',
        pathMatch: 'full',
        title: 'Materias | Portal Académico',
        loadComponent: () =>
          import('./pages/resource-list/admin-resource-list-page').then(
            (component) => component.AdminResourceListPage,
          ),
        data: { resource: 'courses' },
      },
      {
        path: 'courses/new',
        canDeactivate: [pendingChangesGuard],
        title: 'Crear materia | Portal Académico',
        loadComponent: () =>
          import('./pages/course-form/admin-course-form-page').then(
            (component) => component.AdminCourseFormPage,
          ),
      },
      {
        path: 'courses/:id/edit',
        canDeactivate: [pendingChangesGuard],
        title: 'Editar materia | Portal Académico',
        loadComponent: () =>
          import('./pages/course-form/admin-course-form-page').then(
            (component) => component.AdminCourseFormPage,
          ),
      },
      {
        path: 'professors',
        pathMatch: 'full',
        title: 'Profesores | Portal Académico',
        loadComponent: () =>
          import('./pages/resource-list/admin-resource-list-page').then(
            (component) => component.AdminResourceListPage,
          ),
        data: { resource: 'professors' },
      },
      {
        path: 'professors/new',
        canDeactivate: [pendingChangesGuard],
        title: 'Crear profesor | Portal Académico',
        loadComponent: () =>
          import('./pages/professor-form/admin-professor-form-page').then(
            (component) => component.AdminProfessorFormPage,
          ),
      },
      {
        path: 'professors/:id/edit',
        canDeactivate: [pendingChangesGuard],
        title: 'Editar profesor | Portal Académico',
        loadComponent: () =>
          import('./pages/professor-form/admin-professor-form-page').then(
            (component) => component.AdminProfessorFormPage,
          ),
      },
    ],
  },
];
