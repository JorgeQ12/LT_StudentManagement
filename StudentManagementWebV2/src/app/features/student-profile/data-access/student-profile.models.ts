import { AccountStatus } from '../../../core/api/api.models';
export interface StudentProfile {
  readonly id: string;
  readonly firstName: string;
  readonly lastName: string;
  readonly documentNumber: string;
  readonly dateOfBirth: string;
  readonly phoneNumber: string;
  readonly email: string;
  readonly status: AccountStatus;
}
export interface UpdateStudentProfileRequest {
  readonly firstName: string;
  readonly lastName: string;
  readonly dateOfBirth: string;
  readonly phoneNumber: string;
  readonly email: string;
}
