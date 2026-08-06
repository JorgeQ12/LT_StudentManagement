import { forkJoin, map, Observable, of, switchMap } from 'rxjs';

import { PageQuery, PagedResponse } from './api.models';

const FILTER_PAGE_SIZE = 100;

export function fetchPagedFiltered$<TItem>(
  fetchPage: (query: PageQuery) => Observable<PagedResponse<TItem>>,
  query: PageQuery,
  matches: (item: TItem) => boolean,
): Observable<PagedResponse<TItem>> {
  const hasFilters = Boolean(query.search?.trim() || query.status || query.academicProgramId);

  if (!hasFilters) {
    return fetchPage(query);
  }

  return fetchPage({ ...query, pageNumber: 1, pageSize: FILTER_PAGE_SIZE }).pipe(
    switchMap((firstPage) => {
      const pageCount = Math.ceil(firstPage.totalCount / firstPage.pageSize);
      const remainingRequests = Array.from({ length: Math.max(0, pageCount - 1) }, (_, index) =>
        fetchPage({ ...query, pageNumber: index + 2, pageSize: firstPage.pageSize }),
      );

      return (remainingRequests.length ? forkJoin(remainingRequests) : of([])).pipe(
        map((remainingPages) => {
          const all = [firstPage, ...remainingPages].flatMap((page) => page.items);
          const filtered = all.filter(matches);
          return slicePage(filtered, query);
        }),
      );
    }),
  );
}

export function fetchAllPaged$<TItem>(
  fetchPage: (query: PageQuery) => Observable<PagedResponse<TItem>>,
  pageSize = FILTER_PAGE_SIZE,
): Observable<readonly TItem[]> {
  return fetchPage({ pageNumber: 1, pageSize }).pipe(
    switchMap((firstPage) => {
      const pageCount = Math.ceil(firstPage.totalCount / firstPage.pageSize);
      const remainingRequests = Array.from({ length: Math.max(0, pageCount - 1) }, (_, index) =>
        fetchPage({ pageNumber: index + 2, pageSize }),
      );

      return (remainingRequests.length ? forkJoin(remainingRequests) : of([])).pipe(
        map((remainingPages) => [firstPage, ...remainingPages].flatMap((page) => page.items)),
      );
    }),
  );
}

export function searchableText(values: readonly (string | null | undefined)[]): string {
  return values.filter(Boolean).join(' ').toLocaleLowerCase('es-CO');
}

function slicePage<TItem>(items: readonly TItem[], query: PageQuery): PagedResponse<TItem> {
  const pageNumber = Math.max(1, query.pageNumber ?? 1);
  const pageSize = Math.max(1, query.pageSize ?? 10);
  const start = (pageNumber - 1) * pageSize;

  return {
    items: items.slice(start, start + pageSize),
    pageNumber,
    pageSize,
    totalCount: items.length,
  };
}
