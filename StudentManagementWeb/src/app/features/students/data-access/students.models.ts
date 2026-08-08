import { AccountStatus, PageQuery } from '../../../core/api/api.models';

export interface Student {
  readonly id: string;
  readonly firstName: string;
  readonly lastName: string;
  readonly documentNumber: string;
  readonly dateOfBirth: string;
  readonly phoneNumber: string;
  readonly email: string;
  readonly status: AccountStatus;
}

export interface StudentQuery extends PageQuery {
  readonly status?: AccountStatus;
}

export interface CreateStudentRequest {
  readonly firstName: string;
  readonly lastName: string;
  readonly documentNumber: string;
  readonly dateOfBirth: string;
  readonly phoneNumber: string;
  readonly email: string;
  readonly password: string;
}

export interface UpdateStudentRequest extends Omit<CreateStudentRequest, 'password'> {
  readonly studentId: string;
}
