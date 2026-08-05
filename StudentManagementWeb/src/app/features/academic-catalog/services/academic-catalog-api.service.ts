import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';

import { API_BASE_URL } from '@core/api/api.config';
import { AcademicProgram, AcademicProgramId, Course } from '@core/api/api.models';
import { API_ROUTES } from '@core/api/api.routes';

import {
  AcademicProgramDto,
  CourseDto,
  mapAcademicProgram,
  mapCourse,
} from './academic-catalog.dto';

@Injectable({ providedIn: 'root' })
export class AcademicCatalogApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = inject(API_BASE_URL);

  getActivePrograms(): Observable<readonly AcademicProgram[]> {
    return this.http
      .get<readonly AcademicProgramDto[]>(this.url(API_ROUTES.academicPrograms.active))
      .pipe(map((programs) => programs.map(mapAcademicProgram)));
  }

  getActiveCourses(academicProgramId: AcademicProgramId): Observable<readonly Course[]> {
    return this.http
      .get<readonly CourseDto[]>(this.url(API_ROUTES.courses.activeByProgram), {
        params: { academicProgramId },
      })
      .pipe(map((courses) => courses.map(mapCourse)));
  }

  private url(path: string): string {
    return `${this.baseUrl}/${path}`;
  }
}
