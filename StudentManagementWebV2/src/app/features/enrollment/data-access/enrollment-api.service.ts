import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../../../core/api/api-base-url.token';
import { ClassmatesByCourse, Enrollment } from './enrollment.models';
@Injectable()
export class EnrollmentApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${inject(API_BASE_URL)}/Enrollments`;
  create(academicProgramId: string, courseIds: readonly string[]): Observable<Enrollment> {
    return this.http.post<Enrollment>(`${this.baseUrl}/CreateCurrentStudentEnrollment`, {
      academicProgramId,
      courseIds,
    });
  }
  getCurrent(): Observable<Enrollment> {
    return this.http.get<Enrollment>(`${this.baseUrl}/GetCurrentStudentEnrollment`);
  }
  replace(courseIds: readonly string[]): Observable<Enrollment> {
    return this.http.put<Enrollment>(`${this.baseUrl}/ReplaceCurrentStudentSelectedCourses`, {
      courseIds,
    });
  }
  cancel(): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/CancelCurrentStudentEnrollment`);
  }
  getClassmates(): Observable<readonly ClassmatesByCourse[]> {
    return this.http.get<readonly ClassmatesByCourse[]>(
      `${this.baseUrl}/GetCurrentStudentClassmatesByCourse`,
    );
  }
}
