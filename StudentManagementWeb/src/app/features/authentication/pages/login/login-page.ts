import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { email, form, FormField, minLength, required, submit } from '@angular/forms/signals';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { NgIcon } from '@ng-icons/core';

import { ApiProblemDetails, LoginRequest } from '@core/api/api.models';
import { fieldError, problemFromError } from '@core/api/api-error';
import { AuthFacade } from '@core/auth/auth.facade';

@Component({
  selector: 'sm-login-page',
  imports: [FormField, NgIcon, RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './login-page.html',
  styleUrl: './login-page.css',
})
export class LoginPage {
  private readonly authFacade = inject(AuthFacade);
  private readonly route = inject(ActivatedRoute);
  private readonly model = signal<LoginRequest>({ email: '', password: '' });

  protected readonly problem = signal<ApiProblemDetails | null>(null);
  protected readonly sessionExpired = signal(
    this.route.snapshot.queryParamMap.get('reason') === 'expired',
  );
  protected readonly loginForm = form(this.model, (path) => {
    required(path.email, { message: 'Ingresa tu correo electrónico.' });
    email(path.email, { message: 'Ingresa un correo electrónico válido.' });
    required(path.password, { message: 'Ingresa tu contraseña.' });
    minLength(path.password, 8, { message: 'La contraseña debe tener al menos 8 caracteres.' });
  });

  protected readonly emailError = computed(
    () =>
      fieldError(this.problem(), 'email') ||
      (this.loginForm.email().touched() ? this.loginForm.email().errors()[0]?.message : null),
  );
  protected readonly passwordError = computed(
    () =>
      fieldError(this.problem(), 'password') ||
      (this.loginForm.password().touched() ? this.loginForm.password().errors()[0]?.message : null),
  );

  protected async onSubmit(event: Event): Promise<void> {
    event.preventDefault();
    this.problem.set(null);
    await submit(this.loginForm, async () => {
      try {
        await this.authFacade.login(this.model());
      } catch (error) {
        this.problem.set(problemFromError(error));
      }
      return undefined;
    });
  }
}
