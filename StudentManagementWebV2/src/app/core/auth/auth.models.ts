import { AccountRole } from '../api/api.models';

export interface AuthenticatedUser {
  readonly accountId: string;
  readonly studentId: string | null;
  readonly email: string;
  readonly role: AccountRole;
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

export interface AntiforgeryTokenResponse {
  readonly token: string;
}

export type SessionStatus = 'unknown' | 'authenticated' | 'anonymous';
