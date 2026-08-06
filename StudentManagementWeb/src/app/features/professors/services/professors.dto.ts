import { CatalogStatus, Professor, ProfessorId } from '@core/api/api.models';

export interface ProfessorDto {
  readonly id: string;
  readonly firstName: string;
  readonly lastName: string;
  readonly status: CatalogStatus;
  readonly assignedCourseCount: number;
}

export function mapProfessor(dto: ProfessorDto): Professor {
  return {
    ...dto,
    id: dto.id as ProfessorId,
  };
}
