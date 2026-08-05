export type Opaque<TValue, TName extends string> = TValue & {
  readonly __opaqueType: TName;
};

export type AccountId = Opaque<string, 'AccountId'>;
export type StudentId = Opaque<string, 'StudentId'>;
export type AcademicProgramId = Opaque<string, 'AcademicProgramId'>;
export type CourseId = Opaque<string, 'CourseId'>;
export type ProfessorId = Opaque<string, 'ProfessorId'>;
export type EnrollmentId = Opaque<string, 'EnrollmentId'>;

export type AccountRole = 'Student' | 'Administrator';
export type AccountStatus = 'Active' | 'Inactive';
export type CatalogStatus = 'Active' | 'Inactive';
export type EnrollmentStatus = 'Active' | 'Cancelled';

export interface AuthenticatedUser {
  readonly accountId: AccountId;
  readonly studentId: StudentId | null;
  readonly email: string;
  readonly role: AccountRole;
}

export interface Student {
  readonly id: StudentId;
  readonly firstName: string;
  readonly lastName: string;
  readonly documentNumber: string;
  readonly dateOfBirth: string;
  readonly phoneNumber: string;
  readonly email: string;
  readonly status: AccountStatus;
}

export interface AcademicProgram {
  readonly id: AcademicProgramId;
  readonly code: string;
  readonly name: string;
  readonly description: string;
  readonly status: CatalogStatus;
}

export interface Course {
  readonly id: CourseId;
  readonly academicProgramId: AcademicProgramId;
  readonly code: string;
  readonly name: string;
  readonly credits: number;
  readonly status: CatalogStatus;
  readonly professorId: ProfessorId | null;
  readonly professorFullName: string | null;
}

export interface Professor {
  readonly id: ProfessorId;
  readonly firstName: string;
  readonly lastName: string;
  readonly status: CatalogStatus;
  readonly assignedCourseCount: number;
}

export interface EnrollmentCourse {
  readonly id: CourseId;
  readonly code: string;
  readonly name: string;
  readonly credits: number;
  readonly professorId: ProfessorId;
  readonly professorFullName: string;
}

export interface Enrollment {
  readonly id: EnrollmentId;
  readonly studentId: StudentId;
  readonly academicProgram: AcademicProgram;
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
  readonly courseId: CourseId;
  readonly courseName: string;
  readonly classmates: readonly Classmate[];
}

export interface PagedResponse<TItem> {
  readonly items: readonly TItem[];
  readonly pageNumber: number;
  readonly pageSize: number;
  readonly totalCount: number;
}

export interface PageQuery {
  readonly pageNumber?: number;
  readonly pageSize?: number;
  readonly search?: string;
  readonly status?: AccountStatus | CatalogStatus | '';
  readonly academicProgramId?: AcademicProgramId;
}

export interface ApiValidationIssue {
  readonly code?: string;
  readonly message: string;
}

export interface ApiProblemDetails {
  readonly type?: string;
  readonly title: string;
  readonly status: number;
  readonly detail?: string;
  readonly code?: string;
  readonly traceId?: string;
  readonly errors?: Readonly<Record<string, readonly (string | ApiValidationIssue)[]>>;
  readonly codes?: Readonly<Record<string, readonly string[]>>;
}

export interface AntiforgeryTokenResponse {
  readonly token: string;
}

export interface LoginRequest {
  readonly email: string;
  readonly password: string;
}

export interface RegisterStudentRequest {
  readonly firstName: string;
  readonly lastName: string;
  readonly documentNumber: string;
  readonly dateOfBirth: string;
  readonly phoneNumber: string;
  readonly email: string;
  readonly password: string;
}

export interface UpdateStudentProfileRequest {
  readonly firstName: string;
  readonly lastName: string;
  readonly dateOfBirth: string;
  readonly phoneNumber: string;
  readonly email: string;
}

export interface AdminUpdateStudentRequest extends UpdateStudentProfileRequest {
  readonly documentNumber: string;
}

export interface EnrollmentRequest {
  readonly academicProgramId: AcademicProgramId;
  readonly courseIds: readonly CourseId[];
}

export interface ReplaceEnrollmentCoursesRequest {
  readonly courseIds: readonly CourseId[];
}

export interface AcademicProgramRequest {
  readonly code: string;
  readonly name: string;
  readonly description: string;
}

export interface CreateCourseRequest {
  readonly academicProgramId: AcademicProgramId;
  readonly code: string;
  readonly name: string;
}

export interface UpdateCourseRequest {
  readonly code: string;
  readonly name: string;
}

export interface ProfessorRequest {
  readonly firstName: string;
  readonly lastName: string;
}

export interface AssignProfessorRequest {
  readonly professorId: ProfessorId;
}
