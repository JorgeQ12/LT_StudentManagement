import { ChangeDetectionStrategy, Component, effect, inject, signal } from '@angular/core';
import { disabled, form, FormField, maxLength, required, submit } from '@angular/forms/signals';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Icon } from '@shared/ui/icon';

import {
  AcademicProgram,
  AcademicProgramId,
  ApiProblemDetails,
  CourseId,
  CreateCourseRequest,
  Professor,
  ProfessorId,
} from '@core/api/api.models';
import { fieldError, problemFromError } from '@core/api/api-error';
import { PendingChangesAware } from '@core/navigation/pending-changes.guard';
import { LoadingState } from '@shared/ui/loading-state';
import { PageHeader } from '@shared/ui/page-header';
import { ToastService } from '@shared/ui/toast';

import { CoursesFacade } from '../../facades/courses.facade';

const EMPTY_COURSE: CreateCourseRequest = {
  academicProgramId: '' as AcademicProgramId,
  code: '',
  name: '',
};

@Component({
  selector: 'sm-course-form-page',
  imports: [FormField, LoadingState, Icon, PageHeader, RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './course-form-page.html',
  styleUrl: './course-form-page.css',
})
export class CourseFormPage implements PendingChangesAware {
  private readonly facade = inject(CoursesFacade);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly toast = inject(ToastService);
  private readonly id = this.route.snapshot.paramMap.get('id') as CourseId | null;
  private readonly originalProfessorId = signal<ProfessorId | null>(null);
  private readonly savedModel = signal('');

  protected readonly isEditing = signal(this.id !== null);
  protected readonly loading = signal(true);
  protected readonly problem = signal<ApiProblemDetails | null>(null);
  protected readonly model = signal<CreateCourseRequest>({ ...EMPTY_COURSE });
  protected readonly programs = signal<readonly AcademicProgram[]>([]);
  protected readonly professors = signal<readonly Professor[]>([]);
  protected readonly selectedProfessorId = signal<ProfessorId | null>(null);
  protected readonly courseForm = form(this.model, (path) => {
    required(path.code, { message: 'Ingresa el código de la materia.' });
    maxLength(path.code, 20, { message: 'El código admite máximo 20 caracteres.' });
    required(path.name, { message: 'Ingresa el nombre de la materia.' });
    maxLength(path.name, 150, { message: 'El nombre admite máximo 150 caracteres.' });
    required(path.academicProgramId, { message: 'Selecciona un programa.' });
    disabled(path.academicProgramId, { when: () => this.isEditing() });
  });

  constructor() {
    this.savedModel.set(this.serializeModel());
    effect(() => {
      const current = this.model();
      const normalized = current.code.toUpperCase().replace(/\s+/g, '');
      if (current.code !== normalized) {
        this.model.update((value) => ({ ...value, code: normalized }));
      }
    });
    void this.loadDependencies();
  }

  protected asProfessorId(value: string): ProfessorId {
    return value as ProfessorId;
  }

  protected errorFor(field: string, touched: boolean, clientMessage?: string): string | null {
    return fieldError(this.problem(), field) || (touched ? clientMessage || null : null);
  }

  hasPendingChanges(): boolean {
    return this.serializeModel() !== this.savedModel();
  }

  protected async onSubmit(event: Event): Promise<void> {
    event.preventDefault();
    this.problem.set(null);
    await submit(this.courseForm, async () => {
      try {
        const professorId = this.selectedProfessorId();
        await this.facade.saveCourse(
          this.id,
          this.model(),
          professorId,
          this.originalProfessorId(),
        );
        this.savedModel.set(this.serializeModel());
        this.toast.success(this.id ? 'Materia actualizada' : 'Materia creada');
        await this.router.navigate(['/admin/courses']);
      } catch (error) {
        this.problem.set(problemFromError(error));
      }
      return undefined;
    });
  }

  private async loadDependencies(): Promise<void> {
    try {
      const { programs, professors, course } = await this.facade.loadCourseForm(this.id);
      this.programs.set(programs);
      this.professors.set(professors);
      if (course) {
        this.model.set({
          academicProgramId: course.academicProgramId,
          code: course.code,
          name: course.name,
        });
        this.selectedProfessorId.set(course.professorId);
        this.originalProfessorId.set(course.professorId);
      }
      this.savedModel.set(this.serializeModel());
    } catch (error) {
      this.problem.set(problemFromError(error));
    } finally {
      this.loading.set(false);
    }
  }

  private serializeModel(): string {
    return JSON.stringify({
      course: this.model(),
      professorId: this.selectedProfessorId(),
    });
  }
}
