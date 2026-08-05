import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';

import { LoginRequest, RegisterStudentRequest } from '@core/api/api.models';

import { AntiforgeryService } from './antiforgery.service';
import { AuthenticationApiService } from './authentication-api.service';
import { SessionState } from './session-state';

@Injectable({ providedIn: 'root' })
export class AuthFacade {
  private readonly api = inject(AuthenticationApiService);
  private readonly sessionState = inject(SessionState);
  private readonly antiforgery = inject(AntiforgeryService);
  private readonly router = inject(Router);
  private initialization?: Promise<void>;

  readonly status = this.sessionState.status;
  readonly user = this.sessionState.user;
  readonly isAuthenticated = this.sessionState.isAuthenticated;

  initialize(): Promise<void> {
    if (
      this.sessionState.status() === 'authenticated' ||
      this.sessionState.status() === 'anonymous'
    ) {
      return Promise.resolve();
    }

    if (!this.initialization) {
      this.sessionState.setLoading();
      this.initialization = firstValueFrom(this.api.getAuthenticatedUser())
        .then((user) => this.sessionState.authenticate(user))
        .catch(() => this.sessionState.clear());
    }

    return this.initialization;
  }

  async login(request: LoginRequest): Promise<void> {
    const user = await firstValueFrom(this.api.login(request));
    this.sessionState.authenticate(user);
    await firstValueFrom(this.antiforgery.refresh());
    await this.router.navigateByUrl(this.homeFor(user.role));
  }

  async register(request: RegisterStudentRequest): Promise<void> {
    const user = await firstValueFrom(this.api.register(request));
    this.sessionState.authenticate(user);
    await firstValueFrom(this.antiforgery.refresh());
    await this.router.navigateByUrl('/student');
  }

  async logout(): Promise<void> {
    try {
      await firstValueFrom(this.api.logout());
    } finally {
      this.clearSession();
      await this.router.navigateByUrl('/login');
    }
  }

  clearSession(): void {
    this.sessionState.clear();
    this.antiforgery.clear();
  }

  homeFor(role: 'Student' | 'Administrator'): string {
    return role === 'Administrator' ? '/admin' : '/student';
  }
}
