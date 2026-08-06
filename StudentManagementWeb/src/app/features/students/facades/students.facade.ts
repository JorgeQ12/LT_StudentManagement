import { inject, Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';

import {
  AdminUpdateStudentRequest,
  PageQuery,
  RegisterStudentRequest,
  Student,
  StudentId,
} from '@core/api/api.models';
import { PagedListStore } from '@core/data/paged-list-store';

import { StudentsApiService } from '../services/students-api.service';

@Injectable()
export class StudentsFacade {
  private readonly api = inject(StudentsApiService);
  private readonly store = new PagedListStore<Student>((query) => this.api.list(query));

  readonly items = this.store.items;
  readonly totalCount = this.store.totalCount;
  readonly loading = this.store.loading;
  readonly error = this.store.error;
  readonly busyId = this.store.busyId;

  load(query: PageQuery): Promise<void> {
    return this.store.load(query);
  }

  load$(query: PageQuery) {
    return this.store.load$(query);
  }

  async changeStatus(item: Student, activate: boolean): Promise<void> {
    this.store.setBusy(item.id);
    try {
      if (activate) {
        await firstValueFrom(this.api.activateStudent(item.id));
      } else {
        await firstValueFrom(this.api.deactivateStudent(item.id));
      }
    } finally {
      this.store.setBusy(null);
    }
  }

  getStudent(id: StudentId): Promise<Student> {
    return firstValueFrom(this.api.getStudent(id));
  }

  createStudent(request: RegisterStudentRequest): Promise<Student> {
    return firstValueFrom(this.api.createStudent(request));
  }

  updateStudent(id: StudentId, request: AdminUpdateStudentRequest): Promise<Student> {
    return firstValueFrom(this.api.updateStudent(id, request));
  }
}
