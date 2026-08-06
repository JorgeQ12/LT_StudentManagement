import { HttpParams } from '@angular/common/http';

import { PageQuery } from './api.models';

export function pageQueryParams(query: PageQuery): HttpParams {
  let params = new HttpParams()
    .set('pageNumber', String(query.pageNumber ?? 1))
    .set('pageSize', String(query.pageSize ?? 10));

  const search = query.search?.trim();
  if (search) {
    params = params.set('search', search);
  }
  if (query.status) {
    params = params.set('status', query.status);
  }
  if (query.academicProgramId) {
    params = params.set('academicProgramId', query.academicProgramId);
  }
  return params;
}
