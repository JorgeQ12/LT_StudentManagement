import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';

import { API_BASE_URL } from '@core/api/api.config';
import {
  PageQuery,
  PagedResponse,
  Professor,
  ProfessorId,
  ProfessorRequest,
} from '@core/api/api.models';
import { pageQueryParams } from '@core/api/http-params';
import { mapPagedResponse, PagedResponseDto } from '@core/api/paged.dto';
import { fetchAllPaged$, fetchPagedFiltered$, searchableText } from '@core/api/paged-resource';
import { API_ROUTES } from '@core/api/api.routes';

import { mapProfessor, ProfessorDto } from './professors.dto';

@Injectable({ providedIn: 'root' })
export class ProfessorsApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = inject(API_BASE_URL);

  list(query: PageQuery): Observable<PagedResponse<Professor>> {
    return fetchPagedFiltered$(
      (pageQuery) => this.getPage$(pageQuery),
      query,
      (professor) => this.matches(professor, query),
    );
  }

  getAll(): Observable<readonly Professor[]> {
    return fetchAllPaged$((pageQuery) => this.getPage$(pageQuery));
  }

  getProfessor(id: ProfessorId): Observable<Professor> {
    return this.http
      .get<ProfessorDto>(this.url(API_ROUTES.administration.professors.byId), {
        params: { professorId: id },
      })
      .pipe(map(mapProfessor));
  }

  createProfessor(request: ProfessorRequest): Observable<Professor> {
    return this.http
      .post<ProfessorDto>(this.url(API_ROUTES.administration.professors.create), request)
      .pipe(map(mapProfessor));
  }

  updateProfessor(id: ProfessorId, request: ProfessorRequest): Observable<Professor> {
    return this.http
      .put<ProfessorDto>(this.url(API_ROUTES.administration.professors.update), {
        professorId: id,
        ...request,
      })
      .pipe(map(mapProfessor));
  }

  activateProfessor(id: ProfessorId): Observable<Professor> {
    return this.http
      .put<ProfessorDto>(this.url(API_ROUTES.administration.professors.activate), {
        professorId: id,
      })
      .pipe(map(mapProfessor));
  }

  deactivateProfessor(id: ProfessorId): Observable<void> {
    return this.http.delete<void>(this.url(API_ROUTES.administration.professors.deactivate), {
      params: { professorId: id },
    });
  }

  private getPage$(query: PageQuery): Observable<PagedResponse<Professor>> {
    return this.http
      .get<PagedResponseDto<ProfessorDto>>(this.url(API_ROUTES.administration.professors.all), {
        params: pageQueryParams(query),
      })
      .pipe(map((page) => mapPagedResponse(page, mapProfessor)));
  }

  private matches(professor: Professor, query: PageQuery): boolean {
    if (query.status && professor.status !== query.status) {
      return false;
    }
    const search = query.search?.trim().toLocaleLowerCase('es-CO');
    if (search) {
      return searchableText([professor.firstName, professor.lastName]).includes(search);
    }
    return true;
  }

  private url(path: string): string {
    return `${this.baseUrl}/${path}`;
  }
}
