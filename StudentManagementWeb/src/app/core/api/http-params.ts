import { HttpParams } from '@angular/common/http';

import { PageQuery } from './api.models';

export function paginationQueryParams(query: PageQuery): HttpParams {
  return new HttpParams()
    .set('pageNumber', String(query.pageNumber ?? 1))
    .set('pageSize', String(query.pageSize ?? 10));
}
