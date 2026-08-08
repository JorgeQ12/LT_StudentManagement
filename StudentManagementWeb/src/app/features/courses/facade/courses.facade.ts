import { inject, Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { finalize, forkJoin, Observable } from 'rxjs';
import { ApiErrorService } from '../../../core/api/api-error.service';
import { PagedResponse } from '../../../core/api/api.models';
import { ModalService } from '../../../shared/services/modal/modal.service';
import {
  AcademicCatalogApiService,
  CatalogAcademicProgram,
} from '../../academic-catalog/public-api';
import { Professor, ProfessorsApiService } from '../../professors/public-api';
import { CoursesApiService } from '../data-access/courses-api.service';
import {
  Course,
  CourseQuery,
  CreateCourseRequest,
  UpdateCourseRequest,
} from '../data-access/courses.models';
@Injectable()
export class CoursesFacade {
  private readonly api = inject(CoursesApiService);
  private readonly catalog = inject(AcademicCatalogApiService);
  private readonly professorsApi = inject(ProfessorsApiService);
  private readonly errors = inject(ApiErrorService);
  private readonly modal = inject(ModalService);
  private readonly router = inject(Router);
  private readonly pageState = signal<PagedResponse<Course>>({
    items: [],
    pageNumber: 1,
    pageSize: 20,
    totalCount: 0,
  });
  private readonly selectedState = signal<Course | null>(null);
  private readonly programsState = signal<readonly CatalogAcademicProgram[]>([]);
  private readonly professorsState = signal<readonly Professor[]>([]);
  private readonly listLoadingState = signal(true);
  private readonly detailLoadingState = signal(false);
  private readonly mutationLoadingState = signal(false);
  private lastQuery: CourseQuery = { pageNumber: 1, pageSize: 20 };
  readonly page = this.pageState.asReadonly();
  readonly selected = this.selectedState.asReadonly();
  readonly programs = this.programsState.asReadonly();
  readonly professors = this.professorsState.asReadonly();
  readonly listLoading = this.listLoadingState.asReadonly();
  readonly detailLoading = this.detailLoadingState.asReadonly();
  readonly mutationLoading = this.mutationLoadingState.asReadonly();
  loadLookups(): void {
    if (this.programsState().length && this.professorsState().length) return;
    this.listLoadingState.set(true);
    forkJoin({
      programs: this.catalog.getActivePrograms(),
      professors: this.professorsApi.getAll({ pageNumber: 1, pageSize: 100, status: 'Active' }),
    })
      .pipe(finalize(() => this.listLoadingState.set(false)))
      .subscribe({
        next: ({ programs, professors }) => {
          this.programsState.set(programs);
          this.professorsState.set(professors.items);
        },
        error: (error: unknown) =>
          this.showError('No fue posible cargar los datos de apoyo', error),
      });
  }
  load(query: CourseQuery, onSuccess?: () => void): void {
    this.lastQuery = query;
    this.listLoadingState.set(true);
    this.api
      .getAll(query)
      .pipe(finalize(() => this.listLoadingState.set(false)))
      .subscribe({
        next: (page) => {
          this.pageState.set(page);
          onSuccess?.();
        },
        error: (error: unknown) => this.showError('No fue posible cargar los cursos', error),
      });
  }
  loadById(id: string): void {
    this.selectedState.set(null);
    this.detailLoadingState.set(true);
    this.api
      .getById(id)
      .pipe(finalize(() => this.detailLoadingState.set(false)))
      .subscribe({
        next: (course) => this.selectedState.set(course),
        error: (error: unknown) => this.showError('No fue posible cargar el curso', error),
      });
  }
  save(request: CreateCourseRequest | UpdateCourseRequest): void {
    this.mutationLoadingState.set(true);
    const operation = 'courseId' in request ? this.api.update(request) : this.api.create(request);
    operation.pipe(finalize(() => this.mutationLoadingState.set(false))).subscribe({
      next: async () => {
        await this.router.navigate(['/admin/courses']);
        this.load(
          this.lastQuery,
          () =>
            void this.modal.success({
              title: 'Curso guardado',
              message: 'La información del curso quedó actualizada.',
            }),
        );
      },
      error: (error: unknown) => this.showError('No fue posible guardar el curso', error),
    });
  }
  assignProfessor(courseId: string, professorId: string): void {
    this.mutationLoadingState.set(true);
    this.api
      .assignProfessor(courseId, professorId)
      .pipe(finalize(() => this.mutationLoadingState.set(false)))
      .subscribe({
        next: async (course) => {
          this.selectedState.set(course);
          await this.modal.success({
            title: 'Profesor asignado',
            message: 'La asignación docente se actualizó correctamente.',
          });
        },
        error: (error: unknown) => this.showError('No fue posible asignar el profesor', error),
      });
  }
  async changeStatus(course: Course): Promise<void> {
    const activating = course.status === 'Inactive';
    const confirmed = await this.modal.confirm({
      title: activating ? 'Activar curso' : 'Desactivar curso',
      message: activating
        ? `¿Quieres activar ${course.name}?`
        : `¿Quieres desactivar ${course.name}? Una matrícula activa puede impedirlo.`,
      confirmText: activating ? 'Activar' : 'Desactivar',
      destructive: !activating,
    });
    if (!confirmed) return;
    this.mutationLoadingState.set(true);
    const operation: Observable<unknown> = activating
      ? this.api.activate(course.id)
      : this.api.deactivate(course.id);
    operation.pipe(finalize(() => this.mutationLoadingState.set(false))).subscribe({
      next: async () => {
        await this.modal.success({
          title: activating ? 'Curso activado' : 'Curso desactivado',
          message: 'El estado se actualizó correctamente.',
        });
        this.load(this.lastQuery);
      },
      error: (error: unknown) => this.showError('No fue posible cambiar el estado', error),
    });
  }
  programName(id: string): string {
    return this.programsState().find((program) => program.id === id)?.name ?? 'Programa';
  }
  private showError(title: string, error: unknown): void {
    void this.modal.error({ title, message: this.errors.getMessage(error) });
  }
}
