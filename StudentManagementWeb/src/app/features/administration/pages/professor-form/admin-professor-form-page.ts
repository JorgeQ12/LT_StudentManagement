import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { form, FormField, maxLength, required, submit } from '@angular/forms/signals';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { NgIcon } from '@ng-icons/core';

import { ApiProblemDetails, ProfessorId, ProfessorRequest } from '@core/api/api.models';
import { fieldError, problemFromError } from '@core/api/api-error';
import { PendingChangesAware } from '@core/navigation/pending-changes.guard';
import { LoadingState } from '@shared/ui/loading-state';
import { PageHeader } from '@shared/ui/page-header';
import { ToastService } from '@shared/ui/toast';

import { AdministrationFacade } from '../../facades/administration.facade';

@Component({
  selector: 'sm-admin-professor-form-page',
  imports: [FormField, LoadingState, NgIcon, PageHeader, RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './admin-professor-form-page.html',
  styleUrl: './admin-professor-form-page.css',
})
export class AdminProfessorFormPage implements PendingChangesAware {
  private readonly facade = inject(AdministrationFacade);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly toast = inject(ToastService);
  private readonly id = this.route.snapshot.paramMap.get('id') as ProfessorId | null;
  private readonly model = signal<ProfessorRequest>({ firstName: '', lastName: '' });
  private readonly savedModel = signal(JSON.stringify({ firstName: '', lastName: '' }));

  protected readonly isEditing = signal(this.id !== null);
  protected readonly loading = signal(this.id !== null);
  protected readonly problem = signal<ApiProblemDetails | null>(null);
  protected readonly assignedCourseCount = signal<number | null>(null);
  protected readonly professorForm = form(this.model, (path) => {
    required(path.firstName, { message: 'Ingresa los nombres.' });
    maxLength(path.firstName, 100, { message: 'Admite máximo 100 caracteres.' });
    required(path.lastName, { message: 'Ingresa los apellidos.' });
    maxLength(path.lastName, 100, { message: 'Admite máximo 100 caracteres.' });
  });

  constructor() {
    if (this.id) {
      void this.loadProfessor(this.id);
    }
  }

  protected errorFor(field: string, touched: boolean, clientMessage?: string): string | null {
    return fieldError(this.problem(), field) || (touched ? clientMessage || null : null);
  }

  hasPendingChanges(): boolean {
    return JSON.stringify(this.model()) !== this.savedModel();
  }

  protected async onSubmit(event: Event): Promise<void> {
    event.preventDefault();
    this.problem.set(null);
    await submit(this.professorForm, async () => {
      try {
        if (this.id) {
          await this.facade.updateProfessor(this.id, this.model());
        } else {
          await this.facade.createProfessor(this.model());
        }
        this.savedModel.set(JSON.stringify(this.model()));
        this.toast.success(this.id ? 'Profesor actualizado' : 'Profesor creado');
        await this.router.navigate(['/admin/professors']);
      } catch (error) {
        this.problem.set(problemFromError(error));
      }
      return undefined;
    });
  }

  private async loadProfessor(id: ProfessorId): Promise<void> {
    try {
      const professor = await this.facade.getProfessor(id);
      const model = { firstName: professor.firstName, lastName: professor.lastName };
      this.model.set(model);
      this.savedModel.set(JSON.stringify(model));
      this.assignedCourseCount.set(professor.assignedCourseCount);
    } catch (error) {
      this.problem.set(problemFromError(error));
    } finally {
      this.loading.set(false);
    }
  }
}
