import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { finalize } from 'rxjs';
import { SKIP_GLOBAL_LOADING } from '../http/http-context';
import { GlobalLoadingService } from './global-loading.service';

export const loadingInterceptor: HttpInterceptorFn = (request, next) => {
  if (request.context.get(SKIP_GLOBAL_LOADING)) {
    return next(request);
  }

  const loading = inject(GlobalLoadingService);
  loading.begin();
  return next(request).pipe(finalize(() => loading.end()));
};
