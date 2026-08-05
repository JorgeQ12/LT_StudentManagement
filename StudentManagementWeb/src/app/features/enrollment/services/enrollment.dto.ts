import {
  AcademicProgram,
  AcademicProgramId,
  CatalogStatus,
  ClassmatesByCourse,
  CourseId,
  Enrollment,
  EnrollmentCourse,
  EnrollmentId,
  EnrollmentStatus,
  ProfessorId,
  StudentId,
} from '@core/api/api.models';

interface AcademicProgramDto {
  readonly id: string;
  readonly code: string;
  readonly name: string;
  readonly description: string;
  readonly status: CatalogStatus;
}

interface EnrollmentCourseDto {
  readonly id: string;
  readonly code: string;
  readonly name: string;
  readonly credits: number;
  readonly professorId: string;
  readonly professorFullName: string;
}

export interface EnrollmentDto {
  readonly id: string;
  readonly studentId: string;
  readonly academicProgram: AcademicProgramDto;
  readonly status: EnrollmentStatus;
  readonly totalCredits: number;
  readonly courses: readonly EnrollmentCourseDto[];
  readonly createdAtUtc: string;
  readonly cancelledAtUtc: string | null;
}

interface ClassmateDto {
  readonly fullName: string;
}

export interface ClassmatesByCourseDto {
  readonly courseId: string;
  readonly courseName: string;
  readonly classmates: readonly ClassmateDto[];
}

export function mapEnrollment(dto: EnrollmentDto): Enrollment {
  return {
    ...dto,
    id: dto.id as EnrollmentId,
    studentId: dto.studentId as StudentId,
    academicProgram: mapAcademicProgram(dto.academicProgram),
    courses: dto.courses.map(mapEnrollmentCourse),
  };
}

export function mapClassmatesByCourse(dto: ClassmatesByCourseDto): ClassmatesByCourse {
  return {
    ...dto,
    courseId: dto.courseId as CourseId,
    classmates: dto.classmates.map((classmate) => ({ fullName: classmate.fullName })),
  };
}

function mapAcademicProgram(dto: AcademicProgramDto): AcademicProgram {
  return {
    ...dto,
    id: dto.id as AcademicProgramId,
  };
}

function mapEnrollmentCourse(dto: EnrollmentCourseDto): EnrollmentCourse {
  return {
    ...dto,
    id: dto.id as CourseId,
    professorId: dto.professorId as ProfessorId,
  };
}
