import { ChangeDetectionStrategy, Component, input, output } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Icon } from '@shared/ui/icon';

import { Pagination } from '@shared/ui/pagination';
import { StatusBadge } from '@shared/ui/status-badge';

export type DataTableCellKind = 'default' | 'strong' | 'code' | 'muted' | 'truncate';

export interface DataTableColumn<TItem> {
  readonly header: string;
  readonly render: (item: TItem) => string;
  readonly kind?: DataTableCellKind;
}

type TableStatus = 'Active' | 'Inactive' | 'Cancelled';

@Component({
  selector: 'sm-data-table',
  imports: [Icon, Pagination, RouterLink, StatusBadge],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './data-table.html',
  styleUrl: './data-table.css',
})
export class DataTable<TItem extends { readonly id: string; readonly status: TableStatus }> {
  readonly columns = input.required<readonly DataTableColumn<TItem>[]>();
  readonly items = input.required<readonly TItem[]>();
  readonly editPath = input.required<string[]>();
  readonly busyId = input<string | null>(null);
  readonly pageNumber = input.required<number>();
  readonly pageSize = input.required<number>();
  readonly totalCount = input.required<number>();
  readonly statusChange = output<TItem>();
  readonly pageChange = output<number>();

  protected editLink(item: TItem): unknown[] {
    return [...this.editPath(), item.id, 'edit'];
  }

  protected cellClass(kind: DataTableCellKind | undefined): string {
    switch (kind) {
      case 'code':
        return 'px-5 py-4 font-bold text-brand-700';
      case 'strong':
        return 'px-5 py-4 font-semibold text-content';
      case 'muted':
        return 'px-5 py-4 text-sm text-content-muted';
      case 'truncate':
        return 'max-w-75 truncate px-5 py-4 text-sm text-content-muted';
      default:
        return 'px-5 py-4 text-content';
    }
  }

  protected cellValue(item: TItem, column: DataTableColumn<TItem>): string {
    return column.render(item);
  }
}
