import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../../../core/api/api-base-url.token';
import { CatalogAcademicProgram, CatalogCourse } from './academic-catalog.models';

@Injectable({ providedIn: 'root' })
export class AcademicCatalogApiService {
  private readonly http = inject(HttpClient);
  private readonly apiBaseUrl = inject(API_BASE_URL);

  getActivePrograms(): Observable<readonly CatalogAcademicProgram[]> {
    return this.http.get<readonly CatalogAcademicProgram[]>(
      `${this.apiBaseUrl}/AcademicPrograms/GetActiveAcademicPrograms`,
    );
  }

  getActiveCourses(academicProgramId: string): Observable<readonly CatalogCourse[]> {
    return this.http.get<readonly CatalogCourse[]>(
      `${this.apiBaseUrl}/Courses/GetActiveCoursesByAcademicProgram`,
      {
        params: new HttpParams().set('academicProgramId', academicProgramId),
      },
    );
  }
}
