import { ChangeDetectionStrategy, Component, computed, input, output } from '@angular/core';
import { NgIcon } from '@ng-icons/core';

@Component({
  selector: 'sm-pagination',
  imports: [NgIcon],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './pagination.html',
  styleUrl: './pagination.css',
})
export class Pagination {
  readonly pageNumber = input.required<number>();
  readonly pageSize = input.required<number>();
  readonly totalCount = input.required<number>();
  readonly pageChange = output<number>();

  protected readonly totalPages = computed(() =>
    Math.max(1, Math.ceil(this.totalCount() / this.pageSize())),
  );
}
