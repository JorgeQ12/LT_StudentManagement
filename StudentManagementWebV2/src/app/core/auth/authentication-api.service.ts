import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../api/api-base-url.token';
import { AuthenticatedUser, LoginRequest, RegisterStudentRequest } from './auth.models';

@Injectable({ providedIn: 'root' })
export class AuthenticationApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${inject(API_BASE_URL)}/Authentication`;

  login(request: LoginRequest): Observable<AuthenticatedUser> {
    return this.http.post<AuthenticatedUser>(`${this.baseUrl}/Login`, request);
  }

  register(request: RegisterStudentRequest): Observable<AuthenticatedUser> {
    return this.http.post<AuthenticatedUser>(`${this.baseUrl}/RegisterStudent`, request);
  }

  logout(): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/Logout`, null);
  }

  getAuthenticatedUser(): Observable<AuthenticatedUser> {
    return this.http.get<AuthenticatedUser>(`${this.baseUrl}/GetAuthenticatedUser`);
  }
}
