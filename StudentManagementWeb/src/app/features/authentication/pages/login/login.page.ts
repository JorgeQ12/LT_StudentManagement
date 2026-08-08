import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ApiErrorService } from '../../../../core/api/api-error.service';
import { AuthFacade } from '../../../../core/auth/auth.facade';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { ModalService } from '../../../../shared/services/modal/modal.service';
import { getControlError } from '../../../../shared/utils/form-errors';

@Component({
  selector: 'app-login-page',
  imports: [ReactiveFormsModule, RouterLink, ButtonComponent],
  templateUrl: './login.page.html',
  styleUrl: './login.page.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LoginPage {
  private readonly formBuilder = inject(FormBuilder);
  private readonly auth = inject(AuthFacade);
  private readonly errors = inject(ApiErrorService);
  private readonly modal = inject(ModalService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  protected readonly loading = this.auth.loading;
  protected readonly controlError = getControlError;
  protected readonly form = this.formBuilder.nonNullable.group({
    email: ['', [Validators.required, Validators.email, Validators.maxLength(254)]],
    password: ['', Validators.required],
  });

  constructor() {
    if (this.route.snapshot.queryParamMap.get('sessionExpired') === '1') {
      queueMicrotask(
        () =>
          void this.modal.warning({
            title: 'Tu sesión terminó',
            message: 'Por seguridad, inicia sesión nuevamente para continuar.',
          }),
      );
    }
  }

  protected submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.auth.login(this.form.getRawValue()).subscribe({
      next: (user) => {
        const returnUrl = this.route.snapshot.queryParamMap.get('returnUrl');
        void this.router.navigateByUrl(
          returnUrl ?? (user.role === 'Administrator' ? '/admin' : '/student'),
        );
      },
      error: (error: unknown) =>
        void this.modal.error({
          title: 'No fue posible iniciar sesión',
          message: this.errors.getMessage(error),
        }),
    });
  }
}
