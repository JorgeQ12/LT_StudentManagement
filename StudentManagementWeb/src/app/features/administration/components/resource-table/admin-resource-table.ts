import { ChangeDetectionStrategy, Component, input, output } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NgIcon } from '@ng-icons/core';

import { AcademicProgram, Course, Professor, Student } from '@core/api/api.models';
import { Pagination } from '@shared/ui/pagination';
import { StatusBadge } from '@shared/ui/status-badge';

import { AdminEntity, AdminResource } from '../../facades/administration.facade';

@Component({
  selector: 'sm-admin-resource-table',
  imports: [NgIcon, Pagination, RouterLink, StatusBadge],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './admin-resource-table.html',
  styleUrl: './admin-resource-table.css',
})
export class AdminResourceTable {
  readonly resource = input.required<AdminResource>();
  readonly items = input.required<readonly AdminEntity[]>();
  readonly busyId = input<string | null>(null);
  readonly pageNumber = input.required<number>();
  readonly pageSize = input.required<number>();
  readonly totalCount = input.required<number>();
  readonly statusChange = output<AdminEntity>();
  readonly pageChange = output<number>();

  protected student(item: AdminEntity): Student {
    return item as Student;
  }

  protected program(item: AdminEntity): AcademicProgram {
    return item as AcademicProgram;
  }

  protected course(item: AdminEntity): Course {
    return item as Course;
  }

  protected professor(item: AdminEntity): Professor {
    return item as Professor;
  }
}
