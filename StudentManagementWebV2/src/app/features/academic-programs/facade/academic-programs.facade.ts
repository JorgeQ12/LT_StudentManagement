import { inject, Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { finalize, Observable } from 'rxjs';
import { ApiErrorService } from '../../../core/api/api-error.service';
import { PagedResponse } from '../../../core/api/api.models';
import { ModalService } from '../../../shared/services/modal/modal.service';
import { AcademicProgramsApiService } from '../data-access/academic-programs-api.service';
import {
  AcademicProgram,
  AcademicProgramQuery,
  CreateAcademicProgramRequest,
  UpdateAcademicProgramRequest,
} from '../data-access/academic-programs.models';

const EMPTY_PAGE: PagedResponse<AcademicProgram> = {
  items: [],
  pageNumber: 1,
  pageSize: 20,
  totalCount: 0,
};

@Injectable()
export class AcademicProgramsFacade {
  private readonly api = inject(AcademicProgramsApiService);
  private readonly errors = inject(ApiErrorService);
  private readonly modal = inject(ModalService);
  private readonly router = inject(Router);
  private readonly pageState = signal<PagedResponse<AcademicProgram>>(EMPTY_PAGE);
  private readonly selectedState = signal<AcademicProgram | null>(null);
  private readonly listLoadingState = signal(false);
  private readonly mutationLoadingState = signal(false);
  private lastQuery: AcademicProgramQuery = { pageNumber: 1, pageSize: 20 };

  readonly page = this.pageState.asReadonly();
  readonly selected = this.selectedState.asReadonly();
  readonly listLoading = this.listLoadingState.asReadonly();
  readonly mutationLoading = this.mutationLoadingState.asReadonly();

  load(query: AcademicProgramQuery): void {
    this.lastQuery = query;
    this.listLoadingState.set(true);
    this.api
      .getAll(query)
      .pipe(finalize(() => this.listLoadingState.set(false)))
      .subscribe({
        next: (page) => this.pageState.set(page),
        error: (error: unknown) => this.showError('No fue posible cargar los programas', error),
      });
  }

  loadById(id: string): void {
    this.selectedState.set(null);
    this.listLoadingState.set(true);
    this.api
      .getById(id)
      .pipe(finalize(() => this.listLoadingState.set(false)))
      .subscribe({
        next: (program) => this.selectedState.set(program),
        error: (error: unknown) => this.showError('No fue posible cargar el programa', error),
      });
  }

  save(request: CreateAcademicProgramRequest | UpdateAcademicProgramRequest): void {
    this.mutationLoadingState.set(true);
    const operation =
      'academicProgramId' in request ? this.api.update(request) : this.api.create(request);
    operation.pipe(finalize(() => this.mutationLoadingState.set(false))).subscribe({
      next: async () => {
        await this.modal.success({
          title: 'Programa guardado',
          message: 'La información del programa quedó actualizada.',
        });
        void this.router.navigate(['/admin/programs']);
      },
      error: (error: unknown) => this.showError('No fue posible guardar el programa', error),
    });
  }

  async changeStatus(program: AcademicProgram): Promise<void> {
    const activating = program.status === 'Inactive';
    const confirmed = await this.modal.confirm({
      title: activating ? 'Activar programa' : 'Desactivar programa',
      message: activating
        ? `¿Quieres activar ${program.name}?`
        : `¿Quieres desactivar ${program.name}? Los cursos activos asociados pueden impedir esta operación.`,
      confirmText: activating ? 'Activar' : 'Desactivar',
      destructive: !activating,
    });
    if (!confirmed) return;

    this.mutationLoadingState.set(true);
    const operation: Observable<unknown> = activating
      ? this.api.activate(program.id)
      : this.api.deactivate(program.id);
    operation.pipe(finalize(() => this.mutationLoadingState.set(false))).subscribe({
      next: async () => {
        await this.modal.success({
          title: activating ? 'Programa activado' : 'Programa desactivado',
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
