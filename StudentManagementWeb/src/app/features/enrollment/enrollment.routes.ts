import { Routes } from '@angular/router';
import { EnrollmentApiService } from './data-access/enrollment-api.service';
import { EnrollmentFacade } from './facade/enrollment.facade';
export const ENROLLMENT_ROUTES: Routes = [
  {
    path: '',
    providers: [EnrollmentApiService, EnrollmentFacade],
    loadComponent: () =>
      import('./pages/enrollment/enrollment.page').then((module) => module.EnrollmentPage),
  },
];
