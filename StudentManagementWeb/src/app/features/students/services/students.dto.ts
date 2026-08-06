import {
  AdminUpdateStudentRequest,
  AccountStatus,
  RegisterStudentRequest,
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

export function mapStudent(dto: StudentDto): Student {
  return {
    ...dto,
    id: dto.id as StudentId,
  };
}

export function toAdminUpdate(value: RegisterStudentRequest): AdminUpdateStudentRequest {
  return {
    firstName: value.firstName,
    lastName: value.lastName,
    documentNumber: value.documentNumber,
    dateOfBirth: value.dateOfBirth,
    phoneNumber: value.phoneNumber,
    email: value.email,
  };
}
