import { HttpErrorResponse } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { catchError, finalize, Observable, of, shareReplay, tap, throwError } from 'rxjs';
import { AntiforgeryService } from './antiforgery.service';
import { AuthenticatedUser, LoginRequest, RegisterStudentRequest } from './auth.models';
import { AuthenticationApiService } from './authentication-api.service';
import { SessionState } from './session-state.service';

@Injectable({ providedIn: 'root' })
export class AuthFacade {
  private readonly api = inject(AuthenticationApiService);
  private readonly session = inject(SessionState);
  private readonly antiforgery = inject(AntiforgeryService);
  private readonly pending = signal(false);
  private sessionRequest$: Observable<AuthenticatedUser | null> | null = null;

  readonly user = this.session.user;
  readonly status = this.session.status;
  readonly isAuthenticated = this.session.isAuthenticated;
  readonly isAdministrator = this.session.isAdministrator;
  readonly isStudent = this.session.isStudent;
  readonly loading = this.pending.asReadonly();

  ensureSession(): Observable<AuthenticatedUser | null> {
    if (this.session.status() === 'authenticated') return of(this.session.user());
    if (this.session.status() === 'anonymous') return of(null);
    if (this.sessionRequest$) return this.sessionRequest$;

    this.sessionRequest$ = this.api.getAuthenticatedUser().pipe(
      tap((user) => this.session.authenticate(user)),
      catchError((error: unknown) => {
        this.session.clear();
        if (error instanceof HttpErrorResponse && error.status === 401) return of(null);
        return of(null);
      }),
      finalize(() => {
        this.sessionRequest$ = null;
      }),
      shareReplay({ bufferSize: 1, refCount: false }),
    );
    return this.sessionRequest$;
  }

  login(request: LoginRequest): Observable<AuthenticatedUser> {
    this.pending.set(true);
    return this.api.login(request).pipe(
      tap((user) => {
        this.session.authenticate(user);
        this.antiforgery.clear();
      }),
      catchError((error: unknown) => throwError(() => error)),
      finalize(() => this.pending.set(false)),
    );
  }

  register(request: RegisterStudentRequest): Observable<AuthenticatedUser> {
    this.pending.set(true);
    return this.api.register(request).pipe(
      tap((user) => {
        this.session.authenticate(user);
        this.antiforgery.clear();
      }),
      catchError((error: unknown) => throwError(() => error)),
      finalize(() => this.pending.set(false)),
    );
  }

  logout(): Observable<void> {
    this.pending.set(true);
    return this.api.logout().pipe(
      tap(() => {
        this.session.clear();
        this.antiforgery.clear();
      }),
      finalize(() => this.pending.set(false)),
    );
  }
}
