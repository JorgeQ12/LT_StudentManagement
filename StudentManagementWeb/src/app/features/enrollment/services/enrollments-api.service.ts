import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';

import { API_BASE_URL } from '@core/api/api.config';
import {
  ClassmatesByCourse,
  Enrollment,
  EnrollmentRequest,
  ReplaceEnrollmentCoursesRequest,
} from '@core/api/api.models';
import { API_ROUTES } from '@core/api/api.routes';

import {
  ClassmatesByCourseDto,
  EnrollmentDto,
  mapClassmatesByCourse,
  mapEnrollment,
} from './enrollment.dto';

@Injectable({ providedIn: 'root' })
export class EnrollmentsApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = inject(API_BASE_URL);

  getCurrent(): Observable<Enrollment> {
    return this.http
      .get<EnrollmentDto>(this.url(API_ROUTES.enrollments.current))
      .pipe(map(mapEnrollment));
  }

  create(request: EnrollmentRequest): Observable<Enrollment> {
    return this.http
      .post<EnrollmentDto>(this.url(API_ROUTES.enrollments.create), request)
      .pipe(map(mapEnrollment));
  }

  replaceSelectedCourses(request: ReplaceEnrollmentCoursesRequest): Observable<Enrollment> {
    return this.http
      .put<EnrollmentDto>(this.url(API_ROUTES.enrollments.replace), request)
      .pipe(map(mapEnrollment));
  }

  cancel(): Observable<void> {
    return this.http.delete<void>(this.url(API_ROUTES.enrollments.cancel));
  }

  getClassmates(): Observable<readonly ClassmatesByCourse[]> {
    return this.http
      .get<readonly ClassmatesByCourseDto[]>(this.url(API_ROUTES.enrollments.classmates))
      .pipe(map((groups) => groups.map(mapClassmatesByCourse)));
  }

  private url(path: string): string {
    return `${this.baseUrl}/${path}`;
  }
}
