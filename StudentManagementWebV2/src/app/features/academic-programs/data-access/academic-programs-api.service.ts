import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../../../core/api/api-base-url.token';
import { PagedResponse } from '../../../core/api/api.models';
import { createPageParams } from '../../../core/api/http-params';
import {
  AcademicProgram,
  AcademicProgramQuery,
  CreateAcademicProgramRequest,
  UpdateAcademicProgramRequest,
} from './academic-programs.models';

@Injectable()
export class AcademicProgramsApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${inject(API_BASE_URL)}/AdministrationAcademicPrograms`;

  getAll(query: AcademicProgramQuery): Observable<PagedResponse<AcademicProgram>> {
    return this.http.get<PagedResponse<AcademicProgram>>(`${this.baseUrl}/GetAllAcademicPrograms`, {
      params: createPageParams(query, { status: query.status }),
    });
  }

  getById(academicProgramId: string): Observable<AcademicProgram> {
    return this.http.get<AcademicProgram>(`${this.baseUrl}/GetAcademicProgramById`, {
      params: new HttpParams().set('academicProgramId', academicProgramId),
    });
  }

  create(request: CreateAcademicProgramRequest): Observable<AcademicProgram> {
    return this.http.post<AcademicProgram>(`${this.baseUrl}/CreateAcademicProgram`, request);
  }

  update(request: UpdateAcademicProgramRequest): Observable<AcademicProgram> {
    return this.http.put<AcademicProgram>(`${this.baseUrl}/UpdateAcademicProgram`, request);
  }

  activate(academicProgramId: string): Observable<AcademicProgram> {
    return this.http.put<AcademicProgram>(`${this.baseUrl}/ActivateAcademicProgram`, {
      academicProgramId,
    });
  }

  deactivate(academicProgramId: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/DeactivateAcademicProgram`, {
      params: new HttpParams().set('academicProgramId', academicProgramId),
    });
  }
}
