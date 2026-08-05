import { AccountStatus, Student, StudentId } from '@core/api/api.models';

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
