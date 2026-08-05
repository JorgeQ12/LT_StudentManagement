import { computed, Injectable, signal } from '@angular/core';

import { AuthenticatedUser } from '@core/api/api.models';

export type SessionStatus = 'unknown' | 'loading' | 'authenticated' | 'anonymous';

@Injectable({ providedIn: 'root' })
export class SessionState {
  private readonly statusState = signal<SessionStatus>('unknown');
  private readonly userState = signal<AuthenticatedUser | null>(null);

  readonly status = computed(() => this.statusState());
  readonly user = computed(() => this.userState());
  readonly isAuthenticated = computed(() => this.statusState() === 'authenticated');

  setLoading(): void {
    this.statusState.set('loading');
  }

  authenticate(user: AuthenticatedUser): void {
    this.userState.set(user);
    this.statusState.set('authenticated');
  }

  clear(): void {
    this.userState.set(null);
    this.statusState.set('anonymous');
  }
}
