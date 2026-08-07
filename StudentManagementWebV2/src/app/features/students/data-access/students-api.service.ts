import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../../../core/api/api-base-url.token';
import { PagedResponse } from '../../../core/api/api.models';
import { createPageParams } from '../../../core/api/http-params';
import {
  CreateStudentRequest,
  Student,
  StudentQuery,
  UpdateStudentRequest,
} from './students.models';

@Injectable()
export class StudentsApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${inject(API_BASE_URL)}/AdministrationStudents`;
  getAll(query: StudentQuery): Observable<PagedResponse<Student>> {
    return this.http.get<PagedResponse<Student>>(`${this.baseUrl}/GetAllStudents`, {
      params: createPageParams(query, { status: query.status }),
    });
  }
  getById(studentId: string): Observable<Student> {
    return this.http.get<Student>(`${this.baseUrl}/GetStudentById`, {
      params: new HttpParams().set('studentId', studentId),
    });
  }
  create(request: CreateStudentRequest): Observable<Student> {
    return this.http.post<Student>(`${this.baseUrl}/CreateStudent`, request);
  }
  update(request: UpdateStudentRequest): Observable<Student> {
    return this.http.put<Student>(`${this.baseUrl}/UpdateStudent`, request);
  }
  activate(studentId: string): Observable<Student> {
    return this.http.put<Student>(`${this.baseUrl}/ActivateStudent`, { studentId });
  }
  deactivate(studentId: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/DeactivateStudent`, {
      params: new HttpParams().set('studentId', studentId),
    });
  }
}
