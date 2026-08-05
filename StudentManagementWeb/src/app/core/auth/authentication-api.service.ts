import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';

import { API_BASE_URL } from '@core/api/api.config';
import {
  AntiforgeryTokenResponse,
  AuthenticatedUser,
  LoginRequest,
  RegisterStudentRequest,
} from '@core/api/api.models';
import { API_ROUTES } from '@core/api/api.routes';

import { AuthenticatedUserDto, mapAuthenticatedUser } from './authentication.dto';

@Injectable({ providedIn: 'root' })
export class AuthenticationApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = inject(API_BASE_URL);

  getAntiforgeryToken(): Observable<AntiforgeryTokenResponse> {
    return this.http.get<AntiforgeryTokenResponse>(this.url(API_ROUTES.authentication.antiforgery));
  }

  register(request: RegisterStudentRequest): Observable<AuthenticatedUser> {
    return this.http
      .post<AuthenticatedUserDto>(this.url(API_ROUTES.authentication.register), request)
      .pipe(map(mapAuthenticatedUser));
  }

  login(request: LoginRequest): Observable<AuthenticatedUser> {
    return this.http
      .post<AuthenticatedUserDto>(this.url(API_ROUTES.authentication.login), request)
      .pipe(map(mapAuthenticatedUser));
  }

  logout(): Observable<void> {
    return this.http.post<void>(this.url(API_ROUTES.authentication.logout), null);
  }

  getAuthenticatedUser(): Observable<AuthenticatedUser> {
    return this.http
      .get<AuthenticatedUserDto>(this.url(API_ROUTES.authentication.authenticatedUser))
      .pipe(map(mapAuthenticatedUser));
  }

  private url(path: string): string {
    return `${this.baseUrl}/${path}`;
  }
}
