import { computed, Injectable, signal } from '@angular/core';
import { AuthenticatedUser, SessionStatus } from './auth.models';

@Injectable({ providedIn: 'root' })
export class SessionState {
  private readonly currentUser = signal<AuthenticatedUser | null>(null);
  private readonly currentStatus = signal<SessionStatus>('unknown');

  readonly user = this.currentUser.asReadonly();
  readonly status = this.currentStatus.asReadonly();
  readonly isAuthenticated = computed(() => this.currentStatus() === 'authenticated');
  readonly isAdministrator = computed(() => this.currentUser()?.role === 'Administrator');
  readonly isStudent = computed(() => this.currentUser()?.role === 'Student');

  authenticate(user: AuthenticatedUser): void {
    this.currentUser.set(user);
    this.currentStatus.set('authenticated');
  }

  clear(): void {
    this.currentUser.set(null);
    this.currentStatus.set('anonymous');
  }
}
