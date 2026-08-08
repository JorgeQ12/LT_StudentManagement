import { CatalogStatus, PageQuery } from '../../../core/api/api.models';
export interface Course {
  readonly id: string;
  readonly academicProgramId: string;
  readonly code: string;
  readonly name: string;
  readonly credits: number;
  readonly status: CatalogStatus;
  readonly professorId: string | null;
  readonly professorFullName: string | null;
}
export interface CourseQuery extends PageQuery {
  readonly status?: CatalogStatus;
  readonly academicProgramId?: string;
}
export interface CreateCourseRequest {
  readonly academicProgramId: string;
  readonly code: string;
  readonly name: string;
}
export interface UpdateCourseRequest {
  readonly courseId: string;
  readonly code: string;
  readonly name: string;
}
