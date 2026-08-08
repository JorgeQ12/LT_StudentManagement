import { CatalogStatus, PageQuery } from '../../../core/api/api.models';

export interface Professor {
  readonly id: string;
  readonly firstName: string;
  readonly lastName: string;
  readonly status: CatalogStatus;
  readonly assignedCourseCount: number;
}

export interface ProfessorQuery extends PageQuery {
  readonly status?: CatalogStatus;
}

export interface CreateProfessorRequest {
  readonly firstName: string;
  readonly lastName: string;
}
export interface UpdateProfessorRequest extends CreateProfessorRequest {
  readonly professorId: string;
}
