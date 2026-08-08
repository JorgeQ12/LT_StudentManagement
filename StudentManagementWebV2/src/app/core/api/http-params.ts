import { HttpParams } from '@angular/common/http';
import { PageQuery } from './api.models';

export function createPageParams(
  query: PageQuery,
  filters: Readonly<Record<string, string | null | undefined>> = {},
): HttpParams {
  let params = new HttpParams().set('pageNumber', query.pageNumber).set('pageSize', query.pageSize);
  if (query.search?.trim()) params = params.set('search', query.search.trim());
  for (const [key, value] of Object.entries(filters)) {
    if (value) params = params.set(key, value);
  }
  return params;
}
