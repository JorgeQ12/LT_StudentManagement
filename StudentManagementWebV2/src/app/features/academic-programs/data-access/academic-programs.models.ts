import { CatalogStatus, PageQuery } from '../../../core/api/api.models';

export interface AcademicProgram {
  readonly id: string;
  readonly code: string;
  readonly name: string;
  readonly description: string;
  readonly status: CatalogStatus;
}

export interface AcademicProgramQuery extends PageQuery {
  readonly status?: CatalogStatus;
}

export interface CreateAcademicProgramRequest {
  readonly code: string;
  readonly name: string;
  readonly description: string;
}

export interface UpdateAcademicProgramRequest extends CreateAcademicProgramRequest {
  readonly academicProgramId: string;
}
