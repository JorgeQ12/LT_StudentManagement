import { ChangeDetectionStrategy, Component, effect, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { FormModalComponent } from '../../../../shared/components/form-modal/form-modal.component';
import { getControlError } from '../../../../shared/utils/form-errors';
import { AcademicProgramsFacade } from '../../facade/academic-programs.facade';

@Component({
  selector: 'app-program-form',
  imports: [ReactiveFormsModule, RouterLink, ButtonComponent, FormModalComponent],
  templateUrl: './program-form.component.html',
  styleUrl: './program-form.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProgramFormComponent {
  protected readonly facade = inject(AcademicProgramsFacade);
  private readonly route = inject(ActivatedRoute);
  private readonly formBuilder = inject(FormBuilder);
  protected readonly id = this.route.snapshot.paramMap.get('id');
  protected readonly controlError = getControlError;
  protected readonly form = this.formBuilder.nonNullable.group({
    code: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(15)]],
    name: ['', [Validators.required, Validators.maxLength(150)]],
    description: ['', [Validators.required, Validators.maxLength(500)]],
  });

  constructor() {
    if (this.id) this.facade.loadById(this.id);
    effect(() => {
      const program = this.facade.selected();
      if (program && program.id === this.id)
        this.form.reset({
          code: program.code,
          name: program.name,
          description: program.description,
        });
    });
  }

  protected submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    const value = this.form.getRawValue();
    this.facade.save(this.id ? { academicProgramId: this.id, ...value } : value);
  }
}
