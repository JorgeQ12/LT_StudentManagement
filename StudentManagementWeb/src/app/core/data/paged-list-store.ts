import { computed, signal } from '@angular/core';
import { catchError, defer, finalize, firstValueFrom, map, Observable, of, tap } from 'rxjs';

import { PageQuery, PagedResponse } from '@core/api/api.models';
import { problemMessage } from '@core/api/api-error';

export class PagedListStore<TItem> {
  private readonly itemsState = signal<readonly TItem[]>([]);
  private readonly totalCountState = signal(0);
  private readonly loadingState = signal(false);
  private readonly errorState = signal<string | null>(null);
  private readonly busyIdState = signal<string | null>(null);

  readonly items = computed(() => this.itemsState());
  readonly totalCount = computed(() => this.totalCountState());
  readonly loading = computed(() => this.loadingState());
  readonly error = computed(() => this.errorState());
  readonly busyId = computed(() => this.busyIdState());

  constructor(private readonly loadPage: (query: PageQuery) => Observable<PagedResponse<TItem>>) {}

  load(query: PageQuery): Promise<void> {
    return firstValueFrom(this.load$(query));
  }

  load$(query: PageQuery): Observable<void> {
    return defer(() => {
      this.loadingState.set(true);
      this.errorState.set(null);

      return this.loadPage(query).pipe(
        tap((page) => {
          this.itemsState.set(page.items);
          this.totalCountState.set(page.totalCount);
        }),
        map(() => undefined),
        catchError((error: unknown) => {
          this.itemsState.set([]);
          this.totalCountState.set(0);
          this.errorState.set(problemMessage(error));
          return of(undefined);
        }),
        finalize(() => this.loadingState.set(false)),
      );
    });
  }

  setBusy(id: string | null): void {
    this.busyIdState.set(id);
  }
}
