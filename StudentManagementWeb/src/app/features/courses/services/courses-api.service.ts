import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';

import { API_BASE_URL } from '@core/api/api.config';
import {
  AssignProfessorRequest,
  Course,
  CourseId,
  CreateCourseRequest,
  PageQuery,
  PagedResponse,
  UpdateCourseRequest,
} from '@core/api/api.models';
import { pageQueryParams } from '@core/api/http-params';
import { mapPagedResponse, PagedResponseDto } from '@core/api/paged.dto';
import { fetchPagedFiltered$, searchableText } from '@core/api/paged-resource';
import { API_ROUTES } from '@core/api/api.routes';

import { CourseDto, mapCourse } from './courses.dto';

@Injectable({ providedIn: 'root' })
export class CoursesApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = inject(API_BASE_URL);

  list(query: PageQuery): Observable<PagedResponse<Course>> {
    return fetchPagedFiltered$(
      (pageQuery) => this.getPage$(pageQuery),
      query,
      (course) => this.matches(course, query),
    );
  }

  getCourse(id: CourseId): Observable<Course> {
    return this.http
      .get<CourseDto>(this.url(API_ROUTES.administration.courses.byId), {
        params: { courseId: id },
      })
      .pipe(map(mapCourse));
  }

  createCourse(request: CreateCourseRequest): Observable<Course> {
    return this.http
      .post<CourseDto>(this.url(API_ROUTES.administration.courses.create), request)
      .pipe(map(mapCourse));
  }

  updateCourse(id: CourseId, request: UpdateCourseRequest): Observable<Course> {
    return this.http
      .put<CourseDto>(this.url(API_ROUTES.administration.courses.update), {
        courseId: id,
        ...request,
      })
      .pipe(map(mapCourse));
  }

  activateCourse(id: CourseId): Observable<Course> {
    return this.http
      .put<CourseDto>(this.url(API_ROUTES.administration.courses.activate), { courseId: id })
      .pipe(map(mapCourse));
  }

  deactivateCourse(id: CourseId): Observable<void> {
    return this.http.delete<void>(this.url(API_ROUTES.administration.courses.deactivate), {
      params: { courseId: id },
    });
  }

  assignProfessor(id: CourseId, request: AssignProfessorRequest): Observable<Course> {
    return this.http
      .put<CourseDto>(this.url(API_ROUTES.administration.courses.assignProfessor), {
        courseId: id,
        ...request,
      })
      .pipe(map(mapCourse));
  }

  private getPage$(query: PageQuery): Observable<PagedResponse<Course>> {
    return this.http
      .get<PagedResponseDto<CourseDto>>(this.url(API_ROUTES.administration.courses.all), {
        params: pageQueryParams(query),
      })
      .pipe(map((page) => mapPagedResponse(page, mapCourse)));
  }

  private matches(course: Course, query: PageQuery): boolean {
    if (query.status && course.status !== query.status) {
      return false;
    }
    if (query.academicProgramId && course.academicProgramId !== query.academicProgramId) {
      return false;
    }
    const search = query.search?.trim().toLocaleLowerCase('es-CO');
    if (search) {
      return searchableText([course.code, course.name, course.professorFullName]).includes(search);
    }
    return true;
  }

  private url(path: string): string {
    return `${this.baseUrl}/${path}`;
  }
}
