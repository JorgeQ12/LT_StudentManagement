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
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { NgIcon } from '@ng-icons/core';

import {
  AdminUpdateStudentRequest,
  ApiProblemDetails,
  RegisterStudentRequest,
  StudentId,
} from '@core/api/api.models';
import { fieldError, problemFromError } from '@core/api/api-error';
import { PendingChangesAware } from '@core/navigation/pending-changes.guard';
import { LoadingState } from '@shared/ui/loading-state';
import { PageHeader } from '@shared/ui/page-header';
import { ToastService } from '@shared/ui/toast';

import { AdministrationFacade } from '../../facades/administration.facade';

const EMPTY_STUDENT: RegisterStudentRequest = {
  firstName: '',
  lastName: '',
  documentNumber: '',
  dateOfBirth: '',
  phoneNumber: '',
  email: '',
  password: '',
};

@Component({
  selector: 'sm-admin-student-form-page',
  imports: [FormField, LoadingState, NgIcon, PageHeader, RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './admin-student-form-page.html',
  styleUrl: './admin-student-form-page.css',
})
export class AdminStudentFormPage implements PendingChangesAware {
  private readonly facade = inject(AdministrationFacade);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly toast = inject(ToastService);
  private readonly id = this.route.snapshot.paramMap.get('id') as StudentId | null;
  private readonly model = signal<RegisterStudentRequest>({ ...EMPTY_STUDENT });
  private readonly savedModel = signal(JSON.stringify(EMPTY_STUDENT));

  protected readonly isEditing = signal(this.id !== null);
  protected readonly loading = signal(this.id !== null);
  protected readonly problem = signal<ApiProblemDetails | null>(null);
  protected readonly maximumBirthDate = new Date().toISOString().slice(0, 10);
  protected readonly studentForm = form(this.model, (path) => {
    required(path.firstName, { message: 'Ingresa los nombres.' });
    required(path.lastName, { message: 'Ingresa los apellidos.' });
    required(path.documentNumber, { message: 'Ingresa el documento.' });
    pattern(path.documentNumber, /^[A-Za-z0-9-]{5,20}$/, {
      message: 'Usa entre 5 y 20 caracteres válidos.',
    });
    required(path.dateOfBirth, { message: 'Selecciona la fecha de nacimiento.' });
    required(path.phoneNumber, { message: 'Ingresa el teléfono.' });
    pattern(path.phoneNumber, /^\+?[0-9]{7,15}$/, { message: 'Ingresa un teléfono válido.' });
    required(path.email, { message: 'Ingresa el correo.' });
    email(path.email, { message: 'Ingresa un correo válido.' });
    required(path.password, {
      when: () => !this.isEditing(),
      message: 'Ingresa una contraseña temporal.',
    });
    minLength(path.password, 8, {
      when: () => !this.isEditing(),
      message: 'La contraseña debe tener al menos 8 caracteres.',
    });
  });

  constructor() {
    if (this.id) {
      void this.loadStudent(this.id);
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
    await submit(this.studentForm, async () => {
      try {
        if (this.id) {
          const value = this.model();
          const update: AdminUpdateStudentRequest = {
            firstName: value.firstName,
            lastName: value.lastName,
            documentNumber: value.documentNumber,
            dateOfBirth: value.dateOfBirth,
            phoneNumber: value.phoneNumber,
            email: value.email,
          };
          await this.facade.updateStudent(this.id, update);
        } else {
          await this.facade.createStudent(this.model());
        }
        this.savedModel.set(JSON.stringify(this.model()));
        this.toast.success(this.id ? 'Estudiante actualizado' : 'Estudiante creado');
        await this.router.navigate(['/admin/students']);
      } catch (error) {
        this.problem.set(problemFromError(error));
      }
      return undefined;
    });
  }

  private async loadStudent(id: StudentId): Promise<void> {
    try {
      const student = await this.facade.getStudent(id);
      const model = {
        firstName: student.firstName,
        lastName: student.lastName,
        documentNumber: student.documentNumber,
        dateOfBirth: student.dateOfBirth,
        phoneNumber: student.phoneNumber,
        email: student.email,
        password: '',
      };
      this.model.set(model);
      this.savedModel.set(JSON.stringify(model));
    } catch (error) {
      this.problem.set(problemFromError(error));
    } finally {
      this.loading.set(false);
    }
  }
}
