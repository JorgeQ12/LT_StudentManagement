import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Icon } from '@shared/ui/icon';

import { EmptyState } from '@shared/ui/empty-state';
import { LoadingState } from '@shared/ui/loading-state';
import { PageHeader } from '@shared/ui/page-header';

import { EnrollmentFacade } from '../../facades/enrollment.facade';

@Component({
  selector: 'sm-classmates-page',
  imports: [EmptyState, LoadingState, Icon, PageHeader, RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './classmates-page.html',
  styleUrl: './classmates-page.css',
})
export class ClassmatesPage {
  protected readonly facade = inject(EnrollmentFacade);
  protected readonly classmates = this.facade.classmates;
  protected readonly loading = this.facade.classmatesLoading;
  protected readonly error = this.facade.classmatesError;

  constructor() {
    void this.loadClassmates();
  }

  protected async loadClassmates(): Promise<void> {
    await this.facade.loadClassmates();
  }

  protected initials(fullName: string): string {
    return fullName
      .split(' ')
      .slice(0, 2)
      .map((part) => part[0])
      .join('')
      .toUpperCase();
  }
}
