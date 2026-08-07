import { Routes } from '@angular/router';
import { StudentProfileApiService } from './data-access/student-profile-api.service';
import { StudentProfileFacade } from './facade/student-profile.facade';
export const STUDENT_ROUTES: Routes = [
  {
    path: '',
    pathMatch: 'full',
    loadComponent: () =>
      import('./pages/dashboard/student-dashboard.page').then(
        (module) => module.StudentDashboardPage,
      ),
  },
  {
    path: 'enrollment',
    loadChildren: () =>
      import('../enrollment/enrollment.routes').then((module) => module.ENROLLMENT_ROUTES),
  },
  {
    path: 'profile',
    providers: [StudentProfileApiService, StudentProfileFacade],
    loadComponent: () =>
      import('./pages/profile/student-profile.page').then((module) => module.StudentProfilePage),
  },
];
