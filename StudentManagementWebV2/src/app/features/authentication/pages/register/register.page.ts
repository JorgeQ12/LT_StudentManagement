import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { ApiErrorService } from '../../../../core/api/api-error.service';
import { AuthFacade } from '../../../../core/auth/auth.facade';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { DatePickerComponent } from '../../../../shared/components/date-picker/date-picker.component';
import { ModalService } from '../../../../shared/services/modal/modal.service';
import { yesterdayIsoDate } from '../../../../shared/utils/dates';
import {
  getControlError,
  passwordStrengthValidator,
  pastDateValidator,
} from '../../../../shared/utils/form-errors';

@Component({
  selector: 'app-register-page',
  imports: [ReactiveFormsModule, RouterLink, ButtonComponent, DatePickerComponent],
  templateUrl: './register.page.html',
  styleUrl: './register.page.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RegisterPage {
  private readonly formBuilder = inject(FormBuilder);
  private readonly auth = inject(AuthFacade);
  private readonly errors = inject(ApiErrorService);
  private readonly modal = inject(ModalService);
  private readonly router = inject(Router);

  protected readonly loading = this.auth.loading;
  protected readonly maxBirthDate = yesterdayIsoDate();
  protected readonly controlError = getControlError;
  protected readonly form = this.formBuilder.nonNullable.group({
    firstName: ['', [Validators.required, Validators.maxLength(100)]],
    lastName: ['', [Validators.required, Validators.maxLength(100)]],
    documentNumber: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(30)]],
    dateOfBirth: ['', [Validators.required, pastDateValidator]],
    phoneNumber: [
      '',
      [Validators.required, Validators.maxLength(30), Validators.pattern(/^[+0-9 ()-]{7,30}$/)],
    ],
    email: ['', [Validators.required, Validators.email, Validators.maxLength(254)]],
    password: ['', [Validators.required, passwordStrengthValidator]],
  });

  protected submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.auth.register(this.form.getRawValue()).subscribe({
      next: async () => {
        await this.modal.success({
          title: 'Cuenta creada',
          message: 'Tu cuenta estudiantil está lista. Ya puedes realizar tu matrícula.',
          confirmText: 'Continuar',
        });
        void this.router.navigate(['/student']);
      },
      error: (error: unknown) =>
        void this.modal.error({
          title: 'No fue posible crear la cuenta',
          message: this.errors.getMessage(error),
        }),
    });
  }
}
