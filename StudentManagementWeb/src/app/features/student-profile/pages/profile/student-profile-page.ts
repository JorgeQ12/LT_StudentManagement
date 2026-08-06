import { ChangeDetectionStrategy, Component, effect, inject, signal } from '@angular/core';
import { email, form, FormField, pattern, required, submit } from '@angular/forms/signals';
import { Router } from '@angular/router';
import { Icon } from '@shared/ui/icon';
import { firstValueFrom } from 'rxjs';

import { ApiProblemDetails, UpdateStudentProfileRequest } from '@core/api/api.models';
import { fieldError, problemFromError } from '@core/api/api-error';
import { AuthFacade } from '@core/auth/auth.facade';
import { PendingChangesAware } from '@core/navigation/pending-changes.guard';
import { ConfirmDialogService } from '@shared/ui/confirm-dialog';
import { LoadingState } from '@shared/ui/loading-state';
import { PageHeader } from '@shared/ui/page-header';
import { StatusBadge } from '@shared/ui/status-badge';
import { ToastService } from '@shared/ui/toast';

import { StudentWorkspaceFacade } from '../../facades/student-workspace.facade';

const EMPTY_PROFILE: UpdateStudentProfileRequest = {
  firstName: '',
  lastName: '',
  dateOfBirth: '',
  phoneNumber: '',
  email: '',
};

@Component({
  selector: 'sm-student-profile-page',
  imports: [FormField, LoadingState, Icon, PageHeader, StatusBadge],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './student-profile-page.html',
  styleUrl: './student-profile-page.css',
})
export class StudentProfilePage implements PendingChangesAware {
  private readonly authFacade = inject(AuthFacade);
  private readonly router = inject(Router);
  private readonly confirmDialog = inject(ConfirmDialogService);
  private readonly toast = inject(ToastService);
  private readonly model = signal<UpdateStudentProfileRequest>({ ...EMPTY_PROFILE });
  private readonly initialized = signal(false);
  private readonly savedModel = signal(JSON.stringify(EMPTY_PROFILE));

  protected readonly facade = inject(StudentWorkspaceFacade);
  protected readonly problem = signal<ApiProblemDetails | null>(null);
  protected readonly deactivating = signal(false);
  protected readonly maximumBirthDate = new Date().toISOString().slice(0, 10);
  protected readonly profileForm = form(this.model, (path) => {
    required(path.firstName, { message: 'Ingresa tus nombres.' });
    required(path.lastName, { message: 'Ingresa tus apellidos.' });
    required(path.dateOfBirth, { message: 'Selecciona tu fecha de nacimiento.' });
    required(path.phoneNumber, { message: 'Ingresa tu teléfono.' });
    pattern(path.phoneNumber, /^\+?[0-9]{7,15}$/, { message: 'Ingresa un teléfono válido.' });
    required(path.email, { message: 'Ingresa tu correo electrónico.' });
    email(path.email, { message: 'Ingresa un correo electrónico válido.' });
  });

  constructor() {
    void this.facade.load();
    effect(() => {
      const profile = this.facade.profile();
      if (profile && !this.initialized()) {
        const model = {
          firstName: profile.firstName,
          lastName: profile.lastName,
          dateOfBirth: profile.dateOfBirth,
          phoneNumber: profile.phoneNumber,
          email: profile.email,
        };
        this.model.set(model);
        this.savedModel.set(JSON.stringify(model));
        this.initialized.set(true);
      }
    });
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
    await submit(this.profileForm, async () => {
      try {
        await this.facade.updateProfile(this.model());
        this.savedModel.set(JSON.stringify(this.model()));
        this.toast.success('Perfil actualizado', 'Tus cambios se guardaron correctamente.');
      } catch (error) {
        this.problem.set(problemFromError(error));
      }
      return undefined;
    });
  }

  protected async deactivateAccount(): Promise<void> {
    const confirmed = await firstValueFrom(
      this.confirmDialog.open({
        title: '¿Desactivar tu cuenta?',
        message:
          'Esta acción cancelará tu inscripción activa y cerrará la sesión. Administración podrá reactivar tu cuenta posteriormente.',
        confirmLabel: 'Sí, desactivar',
        tone: 'danger',
      }),
    );
    if (!confirmed) {
      return;
    }

    this.deactivating.set(true);
    try {
      await this.facade.deactivateAccount();
      this.authFacade.clearSession();
      await this.router.navigate(['/login']);
    } catch (error) {
      const problem = problemFromError(error);
      this.toast.error(problem.title, problem.detail);
    } finally {
      this.deactivating.set(false);
    }
  }
}
