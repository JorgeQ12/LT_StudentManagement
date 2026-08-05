import { ChangeDetectionStrategy, Component, effect, inject, signal } from '@angular/core';
import { form, FormField, maxLength, required, submit } from '@angular/forms/signals';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { NgIcon } from '@ng-icons/core';

import { AcademicProgramId, AcademicProgramRequest, ApiProblemDetails } from '@core/api/api.models';
import { fieldError, problemFromError } from '@core/api/api-error';
import { PendingChangesAware } from '@core/navigation/pending-changes.guard';
import { LoadingState } from '@shared/ui/loading-state';
import { PageHeader } from '@shared/ui/page-header';
import { ToastService } from '@shared/ui/toast';

import { AdministrationFacade } from '../../facades/administration.facade';

const EMPTY_PROGRAM: AcademicProgramRequest = { code: '', name: '', description: '' };

@Component({
  selector: 'sm-admin-program-form-page',
  imports: [FormField, LoadingState, NgIcon, PageHeader, RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './admin-program-form-page.html',
  styleUrl: './admin-program-form-page.css',
})
export class AdminProgramFormPage implements PendingChangesAware {
  private readonly facade = inject(AdministrationFacade);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly toast = inject(ToastService);
  private readonly id = this.route.snapshot.paramMap.get('id') as AcademicProgramId | null;

  protected readonly isEditing = signal(this.id !== null);
  protected readonly loading = signal(this.id !== null);
  protected readonly problem = signal<ApiProblemDetails | null>(null);
  protected readonly model = signal<AcademicProgramRequest>({ ...EMPTY_PROGRAM });
  private readonly savedModel = signal(JSON.stringify(EMPTY_PROGRAM));
  protected readonly programForm = form(this.model, (path) => {
    required(path.code, { message: 'Ingresa el código del programa.' });
    maxLength(path.code, 20, { message: 'El código admite máximo 20 caracteres.' });
    required(path.name, { message: 'Ingresa el nombre del programa.' });
    maxLength(path.name, 150, { message: 'El nombre admite máximo 150 caracteres.' });
    required(path.description, { message: 'Ingresa una descripción.' });
    maxLength(path.description, 500, { message: 'La descripción admite máximo 500 caracteres.' });
  });

  constructor() {
    effect(() => {
      const current = this.model();
      const normalized = current.code.toUpperCase().replace(/\s+/g, '');
      if (current.code !== normalized) {
        this.model.update((value) => ({ ...value, code: normalized }));
      }
    });
    if (this.id) {
      void this.loadProgram(this.id);
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
    await submit(this.programForm, async () => {
      try {
        if (this.id) {
          await this.facade.updateAcademicProgram(this.id, this.model());
        } else {
          await this.facade.createAcademicProgram(this.model());
        }
        this.savedModel.set(JSON.stringify(this.model()));
        this.toast.success(this.id ? 'Programa actualizado' : 'Programa creado');
        await this.router.navigate(['/admin/academic-programs']);
      } catch (error) {
        this.problem.set(problemFromError(error));
      }
      return undefined;
    });
  }

  private async loadProgram(id: AcademicProgramId): Promise<void> {
    try {
      const program = await this.facade.getAcademicProgram(id);
      const model = { code: program.code, name: program.name, description: program.description };
      this.model.set(model);
      this.savedModel.set(JSON.stringify(model));
    } catch (error) {
      this.problem.set(problemFromError(error));
    } finally {
      this.loading.set(false);
    }
  }
}
