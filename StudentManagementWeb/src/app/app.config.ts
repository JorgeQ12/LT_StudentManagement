import { registerLocaleData } from '@angular/common';
import localeEsCo from '@angular/common/locales/es-CO';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { PreloadAllModules, provideRouter, withPreloading } from '@angular/router';
import { provideIcons } from '@ng-icons/core';
import {
  lucideArrowLeft,
  lucideBookCheck,
  lucideBookMarked,
  lucideBookOpen,
  lucideCalendarDays,
  lucideCheck,
  lucideChevronLeft,
  lucideChevronRight,
  lucideCircleAlert,
  lucideCircleCheck,
  lucideClock,
  lucideEye,
  lucideGraduationCap,
  lucideIdCard,
  lucideInbox,
  lucideInfo,
  lucideLayoutDashboard,
  lucideLibrary,
  lucideLoaderCircle,
  lucideLogOut,
  lucideMail,
  lucideMenu,
  lucidePencil,
  lucidePhone,
  lucidePlus,
  lucidePower,
  lucidePowerOff,
  lucidePresentation,
  lucideRefreshCw,
  lucideSave,
  lucideSchool,
  lucideSearch,
  lucideShieldCheck,
  lucideTriangleAlert,
  lucideUserCog,
  lucideUserPlus,
  lucideUserRound,
  lucideUsersRound,
  lucideX,
} from '@ng-icons/lucide';

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
    ...provideIcons({
      lucideArrowLeft,
      lucideBookCheck,
      lucideBookMarked,
      lucideBookOpen,
      lucideCalendarDays,
      lucideCheck,
      lucideChevronLeft,
      lucideChevronRight,
      lucideCircleAlert,
      lucideCircleCheck,
      lucideClock,
      lucideEye,
      lucideGraduationCap,
      lucideIdCard,
      lucideInbox,
      lucideInfo,
      lucideLayoutDashboard,
      lucideLibrary,
      lucideLoaderCircle,
      lucideLogOut,
      lucideMail,
      lucideMenu,
      lucidePencil,
      lucidePhone,
      lucidePlus,
      lucidePower,
      lucidePowerOff,
      lucidePresentation,
      lucideRefreshCw,
      lucideSave,
      lucideSchool,
      lucideSearch,
      lucideShieldCheck,
      lucideTriangleAlert,
      lucideUserCog,
      lucideUserPlus,
      lucideUserRound,
      lucideUsersRound,
      lucideX,
    }),
  ],
};
