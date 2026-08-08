import { ChangeDetectionStrategy, Component, computed, input, output } from '@angular/core';
import { ButtonComponent } from '../button/button.component';

@Component({
  selector: 'app-pagination',
  imports: [ButtonComponent],
  templateUrl: './pagination.component.html',
  styleUrl: './pagination.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PaginationComponent {
  readonly pageNumber = input.required<number>();
  readonly pageSize = input.required<number>();
  readonly totalCount = input.required<number>();
  readonly pageChange = output<number>();

  protected readonly totalPages = computed(() =>
    Math.max(1, Math.ceil(this.totalCount() / this.pageSize())),
  );
  protected readonly from = computed(() =>
    this.totalCount() === 0 ? 0 : (this.pageNumber() - 1) * this.pageSize() + 1,
  );
  protected readonly to = computed(() =>
    Math.min(this.pageNumber() * this.pageSize(), this.totalCount()),
  );
}
