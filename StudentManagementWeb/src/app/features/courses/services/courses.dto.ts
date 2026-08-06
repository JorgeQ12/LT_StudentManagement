import {
  CatalogStatus,
  Course,
  CourseId,
  AcademicProgramId,
  ProfessorId,
} from '@core/api/api.models';

export interface CourseDto {
  readonly id: string;
  readonly academicProgramId: string;
  readonly code: string;
  readonly name: string;
  readonly credits: number;
  readonly status: CatalogStatus;
  readonly professorId: string | null;
  readonly professorFullName: string | null;
}

export function mapCourse(dto: CourseDto): Course {
  return {
    ...dto,
    id: dto.id as CourseId,
    academicProgramId: dto.academicProgramId as AcademicProgramId,
    professorId: dto.professorId as ProfessorId | null,
  };
}
