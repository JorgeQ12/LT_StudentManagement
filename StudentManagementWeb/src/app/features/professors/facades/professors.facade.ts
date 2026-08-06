import { inject, Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';

import { PageQuery, Professor, ProfessorId, ProfessorRequest } from '@core/api/api.models';
import { PagedListStore } from '@core/data/paged-list-store';

import { ProfessorsApiService } from '../services/professors-api.service';

@Injectable()
export class ProfessorsFacade {
  private readonly api = inject(ProfessorsApiService);
  private readonly store = new PagedListStore<Professor>((query) => this.api.list(query));

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

  async changeStatus(item: Professor, activate: boolean): Promise<void> {
    this.store.setBusy(item.id);
    try {
      if (activate) {
        await firstValueFrom(this.api.activateProfessor(item.id));
      } else {
        await firstValueFrom(this.api.deactivateProfessor(item.id));
      }
    } finally {
      this.store.setBusy(null);
    }
  }

  getProfessor(id: ProfessorId): Promise<Professor> {
    return firstValueFrom(this.api.getProfessor(id));
  }

  createProfessor(request: ProfessorRequest): Promise<Professor> {
    return firstValueFrom(this.api.createProfessor(request));
  }

  updateProfessor(id: ProfessorId, request: ProfessorRequest): Promise<Professor> {
    return firstValueFrom(this.api.updateProfessor(id, request));
  }
}
