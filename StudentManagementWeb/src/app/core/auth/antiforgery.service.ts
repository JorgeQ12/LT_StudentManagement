import { HttpClient, HttpContext } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { catchError, finalize, map, Observable, shareReplay, throwError } from 'rxjs';
import { API_BASE_URL } from '../api/api-base-url.token';
import { SKIP_GLOBAL_LOADING } from '../http/http-context';
import { AntiforgeryTokenResponse } from './auth.models';

@Injectable({ providedIn: 'root' })
export class AntiforgeryService {
  private readonly http = inject(HttpClient);
  private readonly endpoint = `${inject(API_BASE_URL)}/Authentication/GenerateAntiforgeryToken`;
  private token: string | null = null;
  private request$: Observable<string> | null = null;

  getToken(): Observable<string> {
    if (this.token) {
      return new Observable<string>((subscriber) => {
        subscriber.next(this.token!);
        subscriber.complete();
      });
    }
    if (this.request$) {
      return this.request$;
    }

    this.request$ = this.http
      .get<AntiforgeryTokenResponse>(this.endpoint, {
        context: new HttpContext().set(SKIP_GLOBAL_LOADING, true),
      })
      .pipe(
        map(({ token }) => {
          this.token = token;
          return token;
        }),
        catchError((error: unknown) => throwError(() => error)),
        finalize(() => {
          this.request$ = null;
        }),
        shareReplay({ bufferSize: 1, refCount: false }),
      );
    return this.request$;
  }

  clear(): void {
    this.token = null;
    this.request$ = null;
  }
}
