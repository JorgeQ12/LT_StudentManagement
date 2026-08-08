import {
  afterNextRender,
  ChangeDetectionStrategy,
  Component,
  inject,
  OnDestroy,
} from '@angular/core';
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
import { ProfessorsFacade } from '../../facade/professors.facade';

@Component({
  selector: 'app-professor-list-page',
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
  templateUrl: './professor-list.page.html',
  styleUrl: './professor-list.page.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProfessorListPage implements OnDestroy {
  protected readonly facade = inject(ProfessorsFacade);
  protected readonly statusOptions = STATUS_FILTER_OPTIONS;
  protected readonly filters = inject(FormBuilder).nonNullable.group({ search: '', status: '' });
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

    afterNextRender(() => this.load(1));
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }

  protected load(pageNumber: number): void {
    const value = this.filters.getRawValue();
    const search = value.search.trim();
    this.facade.load({
      pageNumber,
      pageSize: 20,
      search: search.length >= 3 ? search : undefined,
      status: value.status === 'Active' || value.status === 'Inactive' ? value.status : undefined,
    });
  }
}
