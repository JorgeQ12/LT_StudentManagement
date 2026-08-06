import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';

import { API_BASE_URL } from '@core/api/api.config';
import {
  AdminUpdateStudentRequest,
  PageQuery,
  PagedResponse,
  RegisterStudentRequest,
  Student,
  StudentId,
} from '@core/api/api.models';
import { pageQueryParams } from '@core/api/http-params';
import { mapPagedResponse, PagedResponseDto } from '@core/api/paged.dto';
import { fetchPagedFiltered$, searchableText } from '@core/api/paged-resource';
import { API_ROUTES } from '@core/api/api.routes';

import { mapStudent, StudentDto } from './students.dto';

@Injectable({ providedIn: 'root' })
export class StudentsApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = inject(API_BASE_URL);

  list(query: PageQuery): Observable<PagedResponse<Student>> {
    return fetchPagedFiltered$(
      (pageQuery) => this.getPage$(pageQuery),
      query,
      (student) => this.matches(student, query),
    );
  }

  getStudent(id: StudentId): Observable<Student> {
    return this.http
      .get<StudentDto>(this.url(API_ROUTES.administration.students.byId), {
        params: { studentId: id },
      })
      .pipe(map(mapStudent));
  }

  createStudent(request: RegisterStudentRequest): Observable<Student> {
    return this.http
      .post<StudentDto>(this.url(API_ROUTES.administration.students.create), request)
      .pipe(map(mapStudent));
  }

  updateStudent(id: StudentId, request: AdminUpdateStudentRequest): Observable<Student> {
    return this.http
      .put<StudentDto>(this.url(API_ROUTES.administration.students.update), {
        studentId: id,
        ...request,
      })
      .pipe(map(mapStudent));
  }

  activateStudent(id: StudentId): Observable<Student> {
    return this.http
      .put<StudentDto>(this.url(API_ROUTES.administration.students.activate), { studentId: id })
      .pipe(map(mapStudent));
  }

  deactivateStudent(id: StudentId): Observable<void> {
    return this.http.delete<void>(this.url(API_ROUTES.administration.students.deactivate), {
      params: { studentId: id },
    });
  }

  private getPage$(query: PageQuery): Observable<PagedResponse<Student>> {
    return this.http
      .get<PagedResponseDto<StudentDto>>(this.url(API_ROUTES.administration.students.all), {
        params: pageQueryParams(query),
      })
      .pipe(map((page) => mapPagedResponse(page, mapStudent)));
  }

  private matches(student: Student, query: PageQuery): boolean {
    if (query.status && student.status !== query.status) {
      return false;
    }
    const search = query.search?.trim().toLocaleLowerCase('es-CO');
    if (search) {
      return searchableText([
        student.firstName,
        student.lastName,
        student.documentNumber,
        student.email,
      ]).includes(search);
    }
    return true;
  }

  private url(path: string): string {
    return `${this.baseUrl}/${path}`;
  }
}
