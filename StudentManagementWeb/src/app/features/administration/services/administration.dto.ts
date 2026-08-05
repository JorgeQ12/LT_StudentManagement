import {
  AcademicProgram,
  AcademicProgramId,
  AccountStatus,
  CatalogStatus,
  Course,
  CourseId,
  PagedResponse,
  Professor,
  ProfessorId,
  Student,
  StudentId,
} from '@core/api/api.models';

export interface StudentDto {
  readonly id: string;
  readonly firstName: string;
  readonly lastName: string;
  readonly documentNumber: string;
  readonly dateOfBirth: string;
  readonly phoneNumber: string;
  readonly email: string;
  readonly status: AccountStatus;
}

export interface AcademicProgramDto {
  readonly id: string;
  readonly code: string;
  readonly name: string;
  readonly description: string;
  readonly status: CatalogStatus;
}

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

export interface ProfessorDto {
  readonly id: string;
  readonly firstName: string;
  readonly lastName: string;
  readonly status: CatalogStatus;
  readonly assignedCourseCount: number;
}

export interface PagedResponseDto<TItem> {
  readonly items: readonly TItem[];
  readonly pageNumber: number;
  readonly pageSize: number;
  readonly totalCount: number;
}

export function mapStudent(dto: StudentDto): Student {
  return {
    ...dto,
    id: dto.id as StudentId,
  };
}

export function mapAcademicProgram(dto: AcademicProgramDto): AcademicProgram {
  return {
    ...dto,
    id: dto.id as AcademicProgramId,
  };
}

export function mapCourse(dto: CourseDto): Course {
  return {
    ...dto,
    id: dto.id as CourseId,
    academicProgramId: dto.academicProgramId as AcademicProgramId,
    professorId: dto.professorId as ProfessorId | null,
  };
}

export function mapProfessor(dto: ProfessorDto): Professor {
  return {
    ...dto,
    id: dto.id as ProfessorId,
  };
}

export function mapPagedResponse<TDto, TModel>(
  dto: PagedResponseDto<TDto>,
  mapItem: (item: TDto) => TModel,
): PagedResponse<TModel> {
  return {
    ...dto,
    items: dto.items.map(mapItem),
  };
}
