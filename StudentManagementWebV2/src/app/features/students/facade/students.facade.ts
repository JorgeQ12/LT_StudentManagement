import { inject, Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { finalize, Observable } from 'rxjs';
import { ApiErrorService } from '../../../core/api/api-error.service';
import { PagedResponse } from '../../../core/api/api.models';
import { ModalService } from '../../../shared/services/modal/modal.service';
import { StudentsApiService } from '../data-access/students-api.service';
import {
  CreateStudentRequest,
  Student,
  StudentQuery,
  UpdateStudentRequest,
} from '../data-access/students.models';

@Injectable()
export class StudentsFacade {
  private readonly api = inject(StudentsApiService);
  private readonly errors = inject(ApiErrorService);
  private readonly modal = inject(ModalService);
  private readonly router = inject(Router);
  private readonly pageState = signal<PagedResponse<Student>>({
    items: [],
    pageNumber: 1,
    pageSize: 20,
    totalCount: 0,
  });
  private readonly selectedState = signal<Student | null>(null);
  private readonly listLoadingState = signal(false);
  private readonly mutationLoadingState = signal(false);
  private lastQuery: StudentQuery = { pageNumber: 1, pageSize: 20 };
  readonly page = this.pageState.asReadonly();
  readonly selected = this.selectedState.asReadonly();
  readonly listLoading = this.listLoadingState.asReadonly();
  readonly mutationLoading = this.mutationLoadingState.asReadonly();
  load(query: StudentQuery): void {
    this.lastQuery = query;
    this.listLoadingState.set(true);
    this.api
      .getAll(query)
      .pipe(finalize(() => this.listLoadingState.set(false)))
      .subscribe({
        next: (page) => this.pageState.set(page),
        error: (error: unknown) => this.showError('No fue posible cargar los estudiantes', error),
      });
  }
  loadById(id: string): void {
    this.selectedState.set(null);
    this.listLoadingState.set(true);
    this.api
      .getById(id)
      .pipe(finalize(() => this.listLoadingState.set(false)))
      .subscribe({
        next: (student) => this.selectedState.set(student),
        error: (error: unknown) => this.showError('No fue posible cargar el estudiante', error),
      });
  }
  save(request: CreateStudentRequest | UpdateStudentRequest): void {
    this.mutationLoadingState.set(true);
    const operation = 'studentId' in request ? this.api.update(request) : this.api.create(request);
    operation.pipe(finalize(() => this.mutationLoadingState.set(false))).subscribe({
      next: async () => {
        await this.modal.success({
          title: 'Estudiante guardado',
          message: 'La información del estudiante quedó actualizada.',
        });
        void this.router.navigate(['/admin/students']);
      },
      error: (error: unknown) => this.showError('No fue posible guardar el estudiante', error),
    });
  }
  async changeStatus(student: Student): Promise<void> {
    const activating = student.status === 'Inactive';
    const name = `${student.firstName} ${student.lastName}`;
    const confirmed = await this.modal.confirm({
      title: activating ? 'Activar estudiante' : 'Desactivar estudiante',
      message: activating
        ? `¿Quieres activar la cuenta de ${name}?`
        : `¿Quieres desactivar la cuenta de ${name}?`,
      confirmText: activating ? 'Activar' : 'Desactivar',
      destructive: !activating,
    });
    if (!confirmed) return;
    this.mutationLoadingState.set(true);
    const operation: Observable<unknown> = activating
      ? this.api.activate(student.id)
      : this.api.deactivate(student.id);
    operation.pipe(finalize(() => this.mutationLoadingState.set(false))).subscribe({
      next: async () => {
        await this.modal.success({
          title: activating ? 'Estudiante activado' : 'Estudiante desactivado',
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
