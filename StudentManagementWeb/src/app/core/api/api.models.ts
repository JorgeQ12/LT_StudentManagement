export type AccountRole = 'Student' | 'Administrator';
export type AccountStatus = 'Active' | 'Inactive';
export type CatalogStatus = 'Active' | 'Inactive';
export type EnrollmentStatus = 'Active' | 'Cancelled';

export interface ApiProblemDetails {
  readonly type?: string | null;
  readonly title?: string | null;
  readonly status?: number | null;
  readonly detail?: string | null;
  readonly instance?: string | null;
  readonly code?: string;
  readonly traceId?: string;
  readonly errors?: Readonly<Record<string, readonly string[]>>;
}

export interface PagedResponse<T> {
  readonly items: readonly T[];
  readonly pageNumber: number;
  readonly pageSize: number;
  readonly totalCount: number;
}

export interface PageQuery {
  readonly pageNumber: number;
  readonly pageSize: number;
  readonly search?: string;
}
