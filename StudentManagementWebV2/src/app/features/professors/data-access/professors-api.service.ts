import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../../../core/api/api-base-url.token';
import { PagedResponse } from '../../../core/api/api.models';
import { createPageParams } from '../../../core/api/http-params';
import {
  CreateProfessorRequest,
  Professor,
  ProfessorQuery,
  UpdateProfessorRequest,
} from './professors.models';

@Injectable()
export class ProfessorsApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${inject(API_BASE_URL)}/AdministrationProfessors`;

  getAll(query: ProfessorQuery): Observable<PagedResponse<Professor>> {
    return this.http.get<PagedResponse<Professor>>(`${this.baseUrl}/GetAllProfessors`, {
      params: createPageParams(query, { status: query.status }),
    });
  }

  getById(professorId: string): Observable<Professor> {
    return this.http.get<Professor>(`${this.baseUrl}/GetProfessorById`, {
      params: new HttpParams().set('professorId', professorId),
    });
  }

  create(request: CreateProfessorRequest): Observable<Professor> {
    return this.http.post<Professor>(`${this.baseUrl}/CreateProfessor`, request);
  }

  update(request: UpdateProfessorRequest): Observable<Professor> {
    return this.http.put<Professor>(`${this.baseUrl}/UpdateProfessor`, request);
  }

  activate(professorId: string): Observable<Professor> {
    return this.http.put<Professor>(`${this.baseUrl}/ActivateProfessor`, { professorId });
  }

  deactivate(professorId: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/DeactivateProfessor`, {
      params: new HttpParams().set('professorId', professorId),
    });
  }
}
