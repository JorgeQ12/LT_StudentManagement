import { HttpErrorResponse } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { catchError, finalize, forkJoin, of, throwError } from 'rxjs';
import { ApiErrorService } from '../../../core/api/api-error.service';
import { ModalService } from '../../../shared/services/modal/modal.service';
import {
  AcademicCatalogApiService,
  CatalogAcademicProgram,
  CatalogCourse,
} from '../../academic-catalog/public-api';
import { EnrollmentApiService } from '../data-access/enrollment-api.service';
import { ClassmatesByCourse, Enrollment } from '../data-access/enrollment.models';
@Injectable()
export class EnrollmentFacade {
  private readonly api = inject(EnrollmentApiService);
  private readonly catalog = inject(AcademicCatalogApiService);
  private readonly errors = inject(ApiErrorService);
  private readonly modal = inject(ModalService);
  private readonly enrollmentState = signal<Enrollment | null>(null);
  private readonly programsState = signal<readonly CatalogAcademicProgram[]>([]);
  private readonly coursesState = signal<readonly CatalogCourse[]>([]);
  private readonly classmatesState = signal<readonly ClassmatesByCourse[]>([]);
  private readonly loadingState = signal(true);
  private readonly savingState = signal(false);
  readonly enrollment = this.enrollmentState.asReadonly();
  readonly programs = this.programsState.asReadonly();
  readonly courses = this.coursesState.asReadonly();
  readonly classmates = this.classmatesState.asReadonly();
  readonly loading = this.loadingState.asReadonly();
  readonly saving = this.savingState.asReadonly();
  readonly hasEnrollment = computed(() => this.enrollmentState() !== null);
  load(): void {
    this.loadingState.set(true);
    forkJoin({
      programs: this.catalog.getActivePrograms(),
      enrollment: this.api
        .getCurrent()
        .pipe(
          catchError((error: unknown) =>
            error instanceof HttpErrorResponse && error.status === 404
              ? of(null)
              : throwError(() => error),
          ),
        ),
    })
      .pipe(finalize(() => this.loadingState.set(false)))
      .subscribe({
        next: ({ programs, enrollment }) => {
          this.programsState.set(programs);
          this.enrollmentState.set(enrollment);
          if (enrollment) {
            this.loadCourses(enrollment.academicProgram.id);
            this.loadClassmates();
          } else {
            this.coursesState.set([]);
            this.classmatesState.set([]);
          }
        },
        error: (error: unknown) => this.showError('No fue posible cargar tu matrícula', error),
      });
  }
  loadCourses(academicProgramId: string): void {
    if (!academicProgramId) {
      this.coursesState.set([]);
      return;
    }
    this.loadingState.set(true);
    this.catalog
      .getActiveCourses(academicProgramId)
      .pipe(finalize(() => this.loadingState.set(false)))
      .subscribe({
        next: (courses) => this.coursesState.set(courses),
        error: (error: unknown) => this.showError('No fue posible cargar los cursos', error),
      });
  }
  save(academicProgramId: string, courseIds: readonly string[]): void {
    if (courseIds.length !== 3) {
      void this.modal.warning({
        title: 'Selecciona tres cursos',
        message: 'La matrícula debe incluir exactamente tres cursos.',
      });
      return;
    }
    const professors = this.coursesState()
      .filter((course) => courseIds.includes(course.id))
      .map((course) => course.professorId);
    if (professors.some((id) => !id) || new Set(professors).size !== 3) {
      void this.modal.warning({
        title: 'Profesores diferentes',
        message: 'Los tres cursos deben tener profesores distintos y asignados.',
      });
      return;
    }
    this.savingState.set(true);
    const operation = this.enrollmentState()
      ? this.api.replace(courseIds)
      : this.api.create(academicProgramId, courseIds);
    operation.pipe(finalize(() => this.savingState.set(false))).subscribe({
      next: async (enrollment) => {
        this.enrollmentState.set(enrollment);
        await this.modal.success({
          title: 'Matrícula guardada',
          message: 'Tus tres cursos quedaron registrados correctamente.',
        });
        this.loadClassmates();
      },
      error: (error: unknown) => this.showError('No fue posible guardar la matrícula', error),
    });
  }
  async cancel(): Promise<void> {
    const confirmed = await this.modal.confirm({
      title: 'Cancelar matrícula',
      message: 'Esta acción cancelará tu matrícula activa y quitará los tres cursos seleccionados.',
      confirmText: 'Cancelar matrícula',
      destructive: true,
    });
    if (!confirmed) return;
    this.savingState.set(true);
    this.api
      .cancel()
      .pipe(finalize(() => this.savingState.set(false)))
      .subscribe({
        next: async () => {
          this.enrollmentState.set(null);
          this.coursesState.set([]);
          this.classmatesState.set([]);
          await this.modal.success({
            title: 'Matrícula cancelada',
            message: 'Tu matrícula fue cancelada correctamente.',
          });
        },
        error: (error: unknown) => this.showError('No fue posible cancelar la matrícula', error),
      });
  }
  private loadClassmates(): void {
    this.api.getClassmates().subscribe({
      next: (classmates) => this.classmatesState.set(classmates),
      error: (error: unknown) => this.showError('No fue posible cargar tus compañeros', error),
    });
  }
  private showError(title: string, error: unknown): void {
    void this.modal.error({ title, message: this.errors.getMessage(error) });
  }
}
