import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';

import { API_BASE_URL } from '@core/api/api.config';
import { Student, UpdateStudentProfileRequest } from '@core/api/api.models';
import { API_ROUTES } from '@core/api/api.routes';

import { mapStudent, StudentDto } from './student-profile.dto';

@Injectable({ providedIn: 'root' })
export class StudentProfilesApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = inject(API_BASE_URL);

  getCurrent(): Observable<Student> {
    return this.http
      .get<StudentDto>(this.url(API_ROUTES.studentProfiles.current))
      .pipe(map(mapStudent));
  }

  updateCurrent(request: UpdateStudentProfileRequest): Observable<Student> {
    return this.http
      .put<StudentDto>(this.url(API_ROUTES.studentProfiles.update), request)
      .pipe(map(mapStudent));
  }

  deactivateCurrent(): Observable<void> {
    return this.http.delete<void>(this.url(API_ROUTES.studentProfiles.deactivate));
  }

  private url(path: string): string {
    return `${this.baseUrl}/${path}`;
  }
}
