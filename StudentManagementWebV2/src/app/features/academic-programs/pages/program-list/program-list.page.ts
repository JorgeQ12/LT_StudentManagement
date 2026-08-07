import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { RouterLink, RouterOutlet } from '@angular/router';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { CustomSelectComponent } from '../../../../shared/components/custom-select/custom-select.component';
import { STATUS_FILTER_OPTIONS } from '../../../../shared/components/custom-select/custom-select.options';
import { EmptyStateComponent } from '../../../../shared/components/empty-state/empty-state.component';
import { PageHeaderComponent } from '../../../../shared/components/page-header/page-header.component';
import { PaginationComponent } from '../../../../shared/components/pagination/pagination.component';
import { StatusBadgeComponent } from '../../../../shared/components/status-badge/status-badge.component';
import { AcademicProgramsFacade } from '../../facade/academic-programs.facade';

@Component({
  selector: 'app-program-list-page',
  imports: [
    ReactiveFormsModule,
    RouterLink,
    RouterOutlet,
    ButtonComponent,
    CustomSelectComponent,
    EmptyStateComponent,
    PageHeaderComponent,
    PaginationComponent,
    StatusBadgeComponent,
  ],
  templateUrl: './program-list.page.html',
  styleUrl: './program-list.page.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProgramListPage {
  protected readonly facade = inject(AcademicProgramsFacade);
  protected readonly statusOptions = STATUS_FILTER_OPTIONS;
  private readonly formBuilder = inject(FormBuilder);
  protected readonly filters = this.formBuilder.nonNullable.group({ search: '', status: '' });

  constructor() {
    this.load(1);
  }

  protected load(pageNumber: number): void {
    const filters = this.filters.getRawValue();
    this.facade.load({
      pageNumber,
      pageSize: 20,
      search: filters.search || undefined,
      status:
        filters.status === 'Active' || filters.status === 'Inactive' ? filters.status : undefined,
    });
  }
}
