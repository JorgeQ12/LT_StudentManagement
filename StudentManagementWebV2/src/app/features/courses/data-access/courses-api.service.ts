import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../../../core/api/api-base-url.token';
import { PagedResponse } from '../../../core/api/api.models';
import { createPageParams } from '../../../core/api/http-params';
import { Course, CourseQuery, CreateCourseRequest, UpdateCourseRequest } from './courses.models';
@Injectable()
export class CoursesApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${inject(API_BASE_URL)}/AdministrationCourses`;
  getAll(query: CourseQuery): Observable<PagedResponse<Course>> {
    return this.http.get<PagedResponse<Course>>(`${this.baseUrl}/GetAllCourses`, {
      params: createPageParams(query, {
        status: query.status,
        academicProgramId: query.academicProgramId,
      }),
    });
  }
  getById(courseId: string): Observable<Course> {
    return this.http.get<Course>(`${this.baseUrl}/GetCourseById`, {
      params: new HttpParams().set('courseId', courseId),
    });
  }
  create(request: CreateCourseRequest): Observable<Course> {
    return this.http.post<Course>(`${this.baseUrl}/CreateCourse`, request);
  }
  update(request: UpdateCourseRequest): Observable<Course> {
    return this.http.put<Course>(`${this.baseUrl}/UpdateCourse`, request);
  }
  activate(courseId: string): Observable<Course> {
    return this.http.put<Course>(`${this.baseUrl}/ActivateCourse`, { courseId });
  }
  deactivate(courseId: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/DeactivateCourse`, {
      params: new HttpParams().set('courseId', courseId),
    });
  }
  assignProfessor(courseId: string, professorId: string): Observable<Course> {
    return this.http.put<Course>(`${this.baseUrl}/AssignProfessor`, { courseId, professorId });
  }
}
