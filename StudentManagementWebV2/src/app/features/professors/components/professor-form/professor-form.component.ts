import { ChangeDetectionStrategy, Component, effect, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { FormModalComponent } from '../../../../shared/components/form-modal/form-modal.component';
import { getControlError } from '../../../../shared/utils/form-errors';
import { ProfessorsFacade } from '../../facade/professors.facade';

@Component({
  selector: 'app-professor-form',
  imports: [ReactiveFormsModule, RouterLink, ButtonComponent, FormModalComponent],
  templateUrl: './professor-form.component.html',
  styleUrl: './professor-form.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProfessorFormComponent {
  protected readonly facade = inject(ProfessorsFacade);
  protected readonly id = inject(ActivatedRoute).snapshot.paramMap.get('id');
  protected readonly controlError = getControlError;
  protected readonly form = inject(FormBuilder).nonNullable.group({
    firstName: ['', [Validators.required, Validators.maxLength(100)]],
    lastName: ['', [Validators.required, Validators.maxLength(100)]],
  });
  constructor() {
    if (this.id) this.facade.loadById(this.id);
    effect(() => {
      const professor = this.facade.selected();
      if (professor?.id === this.id)
        this.form.reset({ firstName: professor.firstName, lastName: professor.lastName });
    });
  }
  protected submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    const value = this.form.getRawValue();
    this.facade.save(this.id ? { professorId: this.id, ...value } : value);
  }
}
