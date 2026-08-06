import { registerLocaleData } from '@angular/common';
import localeEsCo from '@angular/common/locales/es-CO';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { PreloadAllModules, provideRouter, withPreloading } from '@angular/router';

import {
  antiforgeryInterceptor,
  credentialsInterceptor,
  sessionInterceptor,
} from '@core/auth/auth.interceptors';

import { routes } from './app.routes';

registerLocaleData(localeEsCo);

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes, withPreloading(PreloadAllModules)),
    provideHttpClient(
      withInterceptors([credentialsInterceptor, antiforgeryInterceptor, sessionInterceptor]),
    ),
  ],
};
