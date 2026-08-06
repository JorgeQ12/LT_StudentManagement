import { AcademicProgram, AcademicProgramId, CatalogStatus } from '@core/api/api.models';

export interface AcademicProgramDto {
  readonly id: string;
  readonly code: string;
  readonly name: string;
  readonly description: string;
  readonly status: CatalogStatus;
}

export function mapAcademicProgram(dto: AcademicProgramDto): AcademicProgram {
  return {
    ...dto,
    id: dto.id as AcademicProgramId,
  };
}
