import { inject, Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';

import {
  AcademicProgram,
  AcademicProgramId,
  AcademicProgramRequest,
  PageQuery,
} from '@core/api/api.models';
import { PagedListStore } from '@core/data/paged-list-store';

import { AcademicProgramsApiService } from '../services/academic-programs-api.service';

@Injectable()
export class AcademicProgramsFacade {
  private readonly api = inject(AcademicProgramsApiService);
  private readonly store = new PagedListStore<AcademicProgram>((query) => this.api.list(query));

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

  async changeStatus(item: AcademicProgram, activate: boolean): Promise<void> {
    this.store.setBusy(item.id);
    try {
      if (activate) {
        await firstValueFrom(this.api.activateProgram(item.id));
      } else {
        await firstValueFrom(this.api.deactivateProgram(item.id));
      }
    } finally {
      this.store.setBusy(null);
    }
  }

  getProgram(id: AcademicProgramId): Promise<AcademicProgram> {
    return firstValueFrom(this.api.getProgram(id));
  }

  createProgram(request: AcademicProgramRequest): Promise<AcademicProgram> {
    return firstValueFrom(this.api.createProgram(request));
  }

  updateProgram(id: AcademicProgramId, request: AcademicProgramRequest): Promise<AcademicProgram> {
    return firstValueFrom(this.api.updateProgram(id, request));
  }
}
