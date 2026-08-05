import { inject, Injectable, signal } from '@angular/core';
import { finalize, map, Observable, of, shareReplay, tap } from 'rxjs';

import { AuthenticationApiService } from './authentication-api.service';

@Injectable({ providedIn: 'root' })
export class AntiforgeryService {
  private readonly authenticationApi = inject(AuthenticationApiService);
  private readonly tokenState = signal<string | null>(null);
  private inFlightRequest?: Observable<string>;

  ensureToken(): Observable<string> {
    const token = this.tokenState();
    if (token) {
      return of(token);
    }

    if (!this.inFlightRequest) {
      this.inFlightRequest = this.authenticationApi.getAntiforgeryToken().pipe(
        map((response) => response.token),
        tap((requestToken) => {
          if (!requestToken) {
            throw new Error('The antiforgery endpoint returned an empty token.');
          }
          this.tokenState.set(requestToken);
        }),
        finalize(() => {
          this.inFlightRequest = undefined;
        }),
        shareReplay({ bufferSize: 1, refCount: false }),
      );
    }

    return this.inFlightRequest;
  }

  refresh(): Observable<string> {
    this.clear();
    return this.ensureToken();
  }

  clear(): void {
    this.tokenState.set(null);
    this.inFlightRequest = undefined;
  }
}
