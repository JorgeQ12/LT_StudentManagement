import { ChangeDetectionStrategy, Component, inject, OnDestroy } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { RouterLink, RouterOutlet } from '@angular/router';
import { debounceTime, distinctUntilChanged, map, Subject, takeUntil } from 'rxjs';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { CustomSelectComponent } from '../../../../shared/components/custom-select/custom-select.component';
import { STATUS_FILTER_OPTIONS } from '../../../../shared/components/custom-select/custom-select.options';
import { EmptyStateComponent } from '../../../../shared/components/empty-state/empty-state.component';
import { IconComponent } from '../../../../shared/components/icon/icon.component';
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
    IconComponent,
    PageHeaderComponent,
    PaginationComponent,
    StatusBadgeComponent,
  ],
  templateUrl: './program-list.page.html',
  styleUrl: './program-list.page.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProgramListPage implements OnDestroy {
  protected readonly facade = inject(AcademicProgramsFacade);
  protected readonly statusOptions = STATUS_FILTER_OPTIONS;
  private readonly formBuilder = inject(FormBuilder);
  protected readonly filters = this.formBuilder.nonNullable.group({ search: '', status: '' });
  private readonly destroyed$ = new Subject<void>();
  private lastAppliedSearch = '';

  constructor() {
    this.filters.controls.search.valueChanges
      .pipe(
        map((value) => value.trim()),
        debounceTime(350),
        map((value) => (value.length >= 3 ? value : '')),
        distinctUntilChanged(),
        takeUntil(this.destroyed$),
      )
      .subscribe((search) => {
        if (search === this.lastAppliedSearch) return;
        this.lastAppliedSearch = search;
        this.load(1);
      });

    this.filters.controls.status.valueChanges
      .pipe(distinctUntilChanged(), takeUntil(this.destroyed$))
      .subscribe(() => this.load(1));

    this.load(1);
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }

  protected load(pageNumber: number): void {
    const filters = this.filters.getRawValue();
    const search = filters.search.trim();
    this.facade.load({
      pageNumber,
      pageSize: 20,
      search: search.length >= 3 ? search : undefined,
      status:
        filters.status === 'Active' || filters.status === 'Inactive' ? filters.status : undefined,
    });
  }
}
