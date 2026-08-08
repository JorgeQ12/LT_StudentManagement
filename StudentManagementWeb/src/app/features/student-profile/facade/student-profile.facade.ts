import { inject, Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';
import { ApiErrorService } from '../../../core/api/api-error.service';
import { AntiforgeryService } from '../../../core/auth/antiforgery.service';
import { SessionState } from '../../../core/auth/session-state.service';
import { ModalService } from '../../../shared/services/modal/modal.service';
import { StudentProfileApiService } from '../data-access/student-profile-api.service';
import { StudentProfile, UpdateStudentProfileRequest } from '../data-access/student-profile.models';
@Injectable()
export class StudentProfileFacade {
  private readonly api = inject(StudentProfileApiService);
  private readonly errors = inject(ApiErrorService);
  private readonly modal = inject(ModalService);
  private readonly session = inject(SessionState);
  private readonly antiforgery = inject(AntiforgeryService);
  private readonly router = inject(Router);
  private readonly profileState = signal<StudentProfile | null>(null);
  private readonly loadingState = signal(false);
  private readonly savingState = signal(false);
  readonly profile = this.profileState.asReadonly();
  readonly loading = this.loadingState.asReadonly();
  readonly saving = this.savingState.asReadonly();
  load(): void {
    this.loadingState.set(true);
    this.api
      .getCurrent()
      .pipe(finalize(() => this.loadingState.set(false)))
      .subscribe({
        next: (profile) => this.profileState.set(profile),
        error: (error: unknown) => this.showError('No fue posible cargar tu perfil', error),
      });
  }
  update(request: UpdateStudentProfileRequest): void {
    this.savingState.set(true);
    this.api
      .update(request)
      .pipe(finalize(() => this.savingState.set(false)))
      .subscribe({
        next: async (profile) => {
          this.profileState.set(profile);
          await this.modal.success({
            title: 'Perfil actualizado',
            message: 'Tus datos personales se guardaron correctamente.',
          });
        },
        error: (error: unknown) => this.showError('No fue posible actualizar el perfil', error),
      });
  }
  async deactivate(): Promise<void> {
    const confirmed = await this.modal.confirm({
      title: 'Desactivar mi cuenta',
      message:
        'Perderás el acceso al portal y tu matrícula activa podrá verse afectada. Esta acción requiere reactivación administrativa.',
      confirmText: 'Desactivar cuenta',
      destructive: true,
    });
    if (!confirmed) return;
    this.savingState.set(true);
    this.api
      .deactivate()
      .pipe(finalize(() => this.savingState.set(false)))
      .subscribe({
        next: async () => {
          this.session.clear();
          this.antiforgery.clear();
          await this.modal.success({
            title: 'Cuenta desactivada',
            message: 'Tu sesión se cerrará ahora.',
          });
          void this.router.navigate(['/auth/login']);
        },
        error: (error: unknown) => this.showError('No fue posible desactivar la cuenta', error),
      });
  }
  private showError(title: string, error: unknown): void {
    void this.modal.error({ title, message: this.errors.getMessage(error) });
  }
}
