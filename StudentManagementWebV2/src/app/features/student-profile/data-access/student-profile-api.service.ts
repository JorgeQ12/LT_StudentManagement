import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../../../core/api/api-base-url.token';
import { StudentProfile, UpdateStudentProfileRequest } from './student-profile.models';
@Injectable()
export class StudentProfileApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${inject(API_BASE_URL)}/StudentProfiles`;
  getCurrent(): Observable<StudentProfile> {
    return this.http.get<StudentProfile>(`${this.baseUrl}/GetCurrentStudentProfile`);
  }
  update(request: UpdateStudentProfileRequest): Observable<StudentProfile> {
    return this.http.put<StudentProfile>(`${this.baseUrl}/UpdateCurrentStudentProfile`, request);
  }
  deactivate(): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/DeactivateCurrentStudentAccount`);
  }
}
