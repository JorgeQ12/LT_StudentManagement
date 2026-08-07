import { inject, Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { finalize, Observable } from 'rxjs';
import { ApiErrorService } from '../../../core/api/api-error.service';
import { PagedResponse } from '../../../core/api/api.models';
import { ModalService } from '../../../shared/services/modal/modal.service';
import { ProfessorsApiService } from '../data-access/professors-api.service';
import {
  CreateProfessorRequest,
  Professor,
  ProfessorQuery,
  UpdateProfessorRequest,
} from '../data-access/professors.models';

@Injectable()
export class ProfessorsFacade {
  private readonly api = inject(ProfessorsApiService);
  private readonly errors = inject(ApiErrorService);
  private readonly modal = inject(ModalService);
  private readonly router = inject(Router);
  private readonly pageState = signal<PagedResponse<Professor>>({
    items: [],
    pageNumber: 1,
    pageSize: 20,
    totalCount: 0,
  });
  private readonly selectedState = signal<Professor | null>(null);
  private readonly listLoadingState = signal(false);
  private readonly mutationLoadingState = signal(false);
  private lastQuery: ProfessorQuery = { pageNumber: 1, pageSize: 20 };

  readonly page = this.pageState.asReadonly();
  readonly selected = this.selectedState.asReadonly();
  readonly listLoading = this.listLoadingState.asReadonly();
  readonly mutationLoading = this.mutationLoadingState.asReadonly();

  load(query: ProfessorQuery): void {
    this.lastQuery = query;
    this.listLoadingState.set(true);
    this.api
      .getAll(query)
      .pipe(finalize(() => this.listLoadingState.set(false)))
      .subscribe({
        next: (page) => this.pageState.set(page),
        error: (error: unknown) => this.showError('No fue posible cargar los profesores', error),
      });
  }

  loadById(id: string): void {
    this.selectedState.set(null);
    this.listLoadingState.set(true);
    this.api
      .getById(id)
      .pipe(finalize(() => this.listLoadingState.set(false)))
      .subscribe({
        next: (professor) => this.selectedState.set(professor),
        error: (error: unknown) => this.showError('No fue posible cargar el profesor', error),
      });
  }

  save(request: CreateProfessorRequest | UpdateProfessorRequest): void {
    this.mutationLoadingState.set(true);
    const operation =
      'professorId' in request ? this.api.update(request) : this.api.create(request);
    operation.pipe(finalize(() => this.mutationLoadingState.set(false))).subscribe({
      next: async () => {
        await this.modal.success({
          title: 'Profesor guardado',
          message: 'La información del profesor quedó actualizada.',
        });
        void this.router.navigate(['/admin/professors']);
      },
      error: (error: unknown) => this.showError('No fue posible guardar el profesor', error),
    });
  }

  async changeStatus(professor: Professor): Promise<void> {
    const activating = professor.status === 'Inactive';
    const fullName = `${professor.firstName} ${professor.lastName}`;
    const confirmed = await this.modal.confirm({
      title: activating ? 'Activar profesor' : 'Desactivar profesor',
      message: activating
        ? `¿Quieres activar a ${fullName}?`
        : `¿Quieres desactivar a ${fullName}? Sus cursos activos pueden impedir la operación.`,
      confirmText: activating ? 'Activar' : 'Desactivar',
      destructive: !activating,
    });
    if (!confirmed) return;
    this.mutationLoadingState.set(true);
    const operation: Observable<unknown> = activating
      ? this.api.activate(professor.id)
      : this.api.deactivate(professor.id);
    operation.pipe(finalize(() => this.mutationLoadingState.set(false))).subscribe({
      next: async () => {
        await this.modal.success({
          title: activating ? 'Profesor activado' : 'Profesor desactivado',
          message: 'El estado se actualizó correctamente.',
        });
        this.load(this.lastQuery);
      },
      error: (error: unknown) => this.showError('No fue posible cambiar el estado', error),
    });
  }

  private showError(title: string, error: unknown): void {
    void this.modal.error({ title, message: this.errors.getMessage(error) });
  }
}
