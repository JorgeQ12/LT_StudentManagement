import { DatePipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NgIcon } from '@ng-icons/core';

import { EmptyState } from '@shared/ui/empty-state';
import { LoadingState } from '@shared/ui/loading-state';
import { PageHeader } from '@shared/ui/page-header';
import { StatusBadge } from '@shared/ui/status-badge';

import { StudentWorkspaceFacade } from '../../facades/student-workspace.facade';

@Component({
  selector: 'sm-student-dashboard-page',
  imports: [DatePipe, EmptyState, LoadingState, NgIcon, PageHeader, RouterLink, StatusBadge],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './student-dashboard-page.html',
  styleUrl: './student-dashboard-page.css',
})
export class StudentDashboardPage {
  protected readonly facade = inject(StudentWorkspaceFacade);

  constructor() {
    void this.facade.load();
  }

  protected reload(): void {
    void this.facade.load(true);
  }
}
