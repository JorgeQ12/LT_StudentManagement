import { PagedResponse } from './api.models';

export interface PagedResponseDto<TItem> {
  readonly items: readonly TItem[];
  readonly pageNumber: number;
  readonly pageSize: number;
  readonly totalCount: number;
}

export function mapPagedResponse<TDto, TModel>(
  dto: PagedResponseDto<TDto>,
  mapItem: (item: TDto) => TModel,
): PagedResponse<TModel> {
  return {
    ...dto,
    items: dto.items.map(mapItem),
  };
}
