import { EnrollmentStatus } from '../../../core/api/api.models';
import { CatalogAcademicProgram } from '../../academic-catalog/public-api';
export interface EnrollmentCourse {
  readonly id: string;
  readonly code: string;
  readonly name: string;
  readonly credits: number;
  readonly professorId: string;
  readonly professorFullName: string;
}
export interface Enrollment {
  readonly id: string;
  readonly studentId: string;
  readonly academicProgram: CatalogAcademicProgram;
  readonly status: EnrollmentStatus;
  readonly totalCredits: number;
  readonly courses: readonly EnrollmentCourse[];
  readonly createdAtUtc: string;
  readonly cancelledAtUtc: string | null;
}
export interface Classmate {
  readonly fullName: string;
}
export interface ClassmatesByCourse {
  readonly courseId: string;
  readonly courseName: string;
  readonly classmates: readonly Classmate[];
}
