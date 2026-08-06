import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import {
  email,
  form,
  FormField,
  minLength,
  pattern,
  required,
  submit,
} from '@angular/forms/signals';
import { RouterLink } from '@angular/router';
import { Icon } from '@shared/ui/icon';

import { ApiProblemDetails, RegisterStudentRequest } from '@core/api/api.models';
import { fieldError, problemFromError } from '@core/api/api-error';
import { AuthFacade } from '@core/auth/auth.facade';

const INITIAL_REGISTRATION: RegisterStudentRequest = {
  firstName: '',
  lastName: '',
  documentNumber: '',
  dateOfBirth: '',
  phoneNumber: '',
  email: '',
  password: '',
};

@Component({
  selector: 'sm-register-page',
  imports: [FormField, Icon, RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './register-page.html',
  styleUrl: './register-page.css',
})
export class RegisterPage {
  private readonly authFacade = inject(AuthFacade);
  private readonly model = signal<RegisterStudentRequest>({ ...INITIAL_REGISTRATION });

  protected readonly problem = signal<ApiProblemDetails | null>(null);
  protected readonly registrationForm = form(this.model, (path) => {
    required(path.firstName, { message: 'Ingresa tus nombres.' });
    required(path.lastName, { message: 'Ingresa tus apellidos.' });
    required(path.documentNumber, { message: 'Ingresa tu documento.' });
    pattern(path.documentNumber, /^[A-Za-z0-9-]{5,20}$/, {
      message: 'Usa entre 5 y 20 caracteres válidos.',
    });
    required(path.dateOfBirth, { message: 'Selecciona tu fecha de nacimiento.' });
    required(path.phoneNumber, { message: 'Ingresa tu teléfono.' });
    pattern(path.phoneNumber, /^\+?[0-9]{7,15}$/, { message: 'Ingresa un teléfono válido.' });
    required(path.email, { message: 'Ingresa tu correo.' });
    email(path.email, { message: 'Ingresa un correo electrónico válido.' });
    required(path.password, { message: 'Crea una contraseña.' });
    minLength(path.password, 8, { message: 'La contraseña debe tener al menos 8 caracteres.' });
  });

  protected errorFor(field: string, touched: boolean, clientMessage?: string): string | null {
    return fieldError(this.problem(), field) || (touched ? clientMessage || null : null);
  }

  protected async onSubmit(event: Event): Promise<void> {
    event.preventDefault();
    this.problem.set(null);
    await submit(this.registrationForm, async () => {
      try {
        await this.authFacade.register(this.model());
      } catch (error) {
        this.problem.set(problemFromError(error));
      }
      return undefined;
    });
  }
}
