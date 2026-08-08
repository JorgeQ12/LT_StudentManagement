import { afterNextRender, ChangeDetectionStrategy, Component, effect, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { DatePickerComponent } from '../../../../shared/components/date-picker/date-picker.component';
import { IconComponent } from '../../../../shared/components/icon/icon.component';
import { PageHeaderComponent } from '../../../../shared/components/page-header/page-header.component';
import { yesterdayIsoDate } from '../../../../shared/utils/dates';
import { getControlError, pastDateValidator } from '../../../../shared/utils/form-errors';
import { StudentProfileFacade } from '../../facade/student-profile.facade';
@Component({
  selector: 'app-student-profile-page',
  imports: [
    ReactiveFormsModule,
    ButtonComponent,
    DatePickerComponent,
    IconComponent,
    PageHeaderComponent,
  ],
  templateUrl: './student-profile.page.html',
  styleUrl: './student-profile.page.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class StudentProfilePage {
  protected readonly facade = inject(StudentProfileFacade);
  protected readonly controlError = getControlError;
  protected readonly maxBirthDate = yesterdayIsoDate();
  protected readonly form = inject(FormBuilder).nonNullable.group({
    firstName: ['', [Validators.required, Validators.maxLength(100)]],
    lastName: ['', [Validators.required, Validators.maxLength(100)]],
    documentNumber: [{ value: '', disabled: true }],
    dateOfBirth: ['', [Validators.required, pastDateValidator]],
    phoneNumber: [
      '',
      [Validators.required, Validators.maxLength(30), Validators.pattern(/^[+0-9 ()-]{7,30}$/)],
    ],
    email: ['', [Validators.required, Validators.email, Validators.maxLength(254)]],
  });
  constructor() {
    afterNextRender(() => this.facade.load());
    effect(() => {
      const profile = this.facade.profile();
      if (profile)
        this.form.reset({
          firstName: profile.firstName,
          lastName: profile.lastName,
          documentNumber: profile.documentNumber,
          dateOfBirth: profile.dateOfBirth,
          phoneNumber: profile.phoneNumber,
          email: profile.email,
        });
    });
  }
  protected submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    const value = this.form.getRawValue();
    this.facade.update({
      firstName: value.firstName,
      lastName: value.lastName,
      dateOfBirth: value.dateOfBirth,
      phoneNumber: value.phoneNumber,
      email: value.email,
    });
  }
}
