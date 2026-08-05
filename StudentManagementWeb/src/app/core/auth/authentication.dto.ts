import { AccountId, AccountRole, AuthenticatedUser, StudentId } from '@core/api/api.models';

export interface AuthenticatedUserDto {
  readonly accountId: string;
  readonly studentId: string | null;
  readonly email: string;
  readonly role: AccountRole;
}

export function mapAuthenticatedUser(dto: AuthenticatedUserDto): AuthenticatedUser {
  return {
    accountId: dto.accountId as AccountId,
    studentId: dto.studentId as StudentId | null,
    email: dto.email,
    role: dto.role,
  };
}
