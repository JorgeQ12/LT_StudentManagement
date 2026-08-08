import { ChangeDetectionStrategy, Component, effect, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { DatePickerComponent } from '../../../../shared/components/date-picker/date-picker.component';
import { FormModalComponent } from '../../../../shared/components/form-modal/form-modal.component';
import { yesterdayIsoDate } from '../../../../shared/utils/dates';
import {
  getControlError,
  passwordStrengthValidator,
  pastDateValidator,
} from '../../../../shared/utils/form-errors';
import { StudentsFacade } from '../../facade/students.facade';
@Component({
  selector: 'app-student-form',
  imports: [
    ReactiveFormsModule,
    RouterLink,
    ButtonComponent,
    DatePickerComponent,
    FormModalComponent,
  ],
  templateUrl: './student-form.component.html',
  styleUrl: './student-form.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class StudentFormComponent {
  protected readonly facade = inject(StudentsFacade);
  protected readonly id = inject(ActivatedRoute).snapshot.paramMap.get('id');
  protected readonly controlError = getControlError;
  protected readonly maxBirthDate = yesterdayIsoDate();
  protected readonly form = inject(FormBuilder).nonNullable.group({
    firstName: ['', [Validators.required, Validators.maxLength(100)]],
    lastName: ['', [Validators.required, Validators.maxLength(100)]],
    documentNumber: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(30)]],
    dateOfBirth: ['', [Validators.required, pastDateValidator]],
    phoneNumber: [
      '',
      [Validators.required, Validators.maxLength(30), Validators.pattern(/^[+0-9 ()-]{7,30}$/)],
    ],
    email: ['', [Validators.required, Validators.email, Validators.maxLength(254)]],
    password: ['', [passwordStrengthValidator]],
  });
  constructor() {
    if (this.id) this.facade.loadById(this.id);
    else {
      this.form.controls.password.addValidators(Validators.required);
      this.form.controls.password.updateValueAndValidity({ emitEvent: false });
    }
    effect(() => {
      const student = this.facade.selected();
      if (student?.id === this.id)
        this.form.reset({
          firstName: student.firstName,
          lastName: student.lastName,
          documentNumber: student.documentNumber,
          dateOfBirth: student.dateOfBirth,
          phoneNumber: student.phoneNumber,
          email: student.email,
          password: '',
        });
    });
  }
  protected submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    const value = this.form.getRawValue();
    if (this.id) {
      this.facade.save({
        studentId: this.id,
        firstName: value.firstName,
        lastName: value.lastName,
        documentNumber: value.documentNumber,
        dateOfBirth: value.dateOfBirth,
        phoneNumber: value.phoneNumber,
        email: value.email,
      });
    } else {
      this.facade.save(value);
    }
  }
}
