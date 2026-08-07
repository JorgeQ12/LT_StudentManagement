import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter, withComponentInputBinding, withInMemoryScrolling } from '@angular/router';

import { routes } from './app.routes';
import {
  credentialsAndAntiforgeryInterceptor,
  sessionInterceptor,
} from './core/auth/auth.interceptors';
import { loadingInterceptor } from './core/loading/loading.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(
      routes,
      withComponentInputBinding(),
      withInMemoryScrolling({ scrollPositionRestoration: 'top' }),
    ),
    provideHttpClient(
      withInterceptors([
        credentialsAndAntiforgeryInterceptor,
        sessionInterceptor,
        loadingInterceptor,
      ]),
    ),
  ],
};
