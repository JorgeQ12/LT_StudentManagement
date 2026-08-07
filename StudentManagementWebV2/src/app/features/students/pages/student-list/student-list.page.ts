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
import { StudentsFacade } from '../../facade/students.facade';
@Component({
  selector: 'app-student-list-page',
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
  templateUrl: './student-list.page.html',
  styleUrl: './student-list.page.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class StudentListPage {
  protected readonly facade = inject(StudentsFacade);
  protected readonly statusOptions = STATUS_FILTER_OPTIONS;
  protected readonly filters = inject(FormBuilder).nonNullable.group({ search: '', status: '' });
  constructor() {
    this.load(1);
  }
  protected load(pageNumber: number): void {
    const value = this.filters.getRawValue();
    this.facade.load({
      pageNumber,
      pageSize: 20,
      search: value.search || undefined,
      status: value.status === 'Active' || value.status === 'Inactive' ? value.status : undefined,
    });
  }
}
