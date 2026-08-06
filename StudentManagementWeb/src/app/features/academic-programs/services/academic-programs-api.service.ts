import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';

import { API_BASE_URL } from '@core/api/api.config';
import {
  AcademicProgram,
  AcademicProgramId,
  AcademicProgramRequest,
  PageQuery,
  PagedResponse,
} from '@core/api/api.models';
import { pageQueryParams } from '@core/api/http-params';
import { mapPagedResponse, PagedResponseDto } from '@core/api/paged.dto';
import { fetchAllPaged$, fetchPagedFiltered$, searchableText } from '@core/api/paged-resource';
import { API_ROUTES } from '@core/api/api.routes';

import { AcademicProgramDto, mapAcademicProgram } from './academic-programs.dto';

@Injectable({ providedIn: 'root' })
export class AcademicProgramsApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = inject(API_BASE_URL);

  list(query: PageQuery): Observable<PagedResponse<AcademicProgram>> {
    return fetchPagedFiltered$(
      (pageQuery) => this.getPage$(pageQuery),
      query,
      (program) => this.matches(program, query),
    );
  }

  getAll(): Observable<readonly AcademicProgram[]> {
    return fetchAllPaged$((pageQuery) => this.getPage$(pageQuery));
  }

  getProgram(id: AcademicProgramId): Observable<AcademicProgram> {
    return this.http
      .get<AcademicProgramDto>(this.url(API_ROUTES.administration.academicPrograms.byId), {
        params: { academicProgramId: id },
      })
      .pipe(map(mapAcademicProgram));
  }

  createProgram(request: AcademicProgramRequest): Observable<AcademicProgram> {
    return this.http
      .post<AcademicProgramDto>(
        this.url(API_ROUTES.administration.academicPrograms.create),
        request,
      )
      .pipe(map(mapAcademicProgram));
  }

  updateProgram(
    id: AcademicProgramId,
    request: AcademicProgramRequest,
  ): Observable<AcademicProgram> {
    return this.http
      .put<AcademicProgramDto>(this.url(API_ROUTES.administration.academicPrograms.update), {
        academicProgramId: id,
        ...request,
      })
      .pipe(map(mapAcademicProgram));
  }

  activateProgram(id: AcademicProgramId): Observable<AcademicProgram> {
    return this.http
      .put<AcademicProgramDto>(this.url(API_ROUTES.administration.academicPrograms.activate), {
        academicProgramId: id,
      })
      .pipe(map(mapAcademicProgram));
  }

  deactivateProgram(id: AcademicProgramId): Observable<void> {
    return this.http.delete<void>(this.url(API_ROUTES.administration.academicPrograms.deactivate), {
      params: { academicProgramId: id },
    });
  }

  private getPage$(query: PageQuery): Observable<PagedResponse<AcademicProgram>> {
    return this.http
      .get<PagedResponseDto<AcademicProgramDto>>(
        this.url(API_ROUTES.administration.academicPrograms.all),
        { params: pageQueryParams(query) },
      )
      .pipe(map((page) => mapPagedResponse(page, mapAcademicProgram)));
  }

  private matches(program: AcademicProgram, query: PageQuery): boolean {
    if (query.status && program.status !== query.status) {
      return false;
    }
    const search = query.search?.trim().toLocaleLowerCase('es-CO');
    if (search) {
      return searchableText([program.code, program.name, program.description]).includes(search);
    }
    return true;
  }

  private url(path: string): string {
    return `${this.baseUrl}/${path}`;
  }
}
