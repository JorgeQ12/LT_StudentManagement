import { ChangeDetectionStrategy, Component, computed, effect, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { CustomSelectComponent } from '../../../../shared/components/custom-select/custom-select.component';
import { FormModalComponent } from '../../../../shared/components/form-modal/form-modal.component';
import { getControlError } from '../../../../shared/utils/form-errors';
import { CoursesFacade } from '../../facade/courses.facade';
@Component({
  selector: 'app-course-form',
  imports: [
    ReactiveFormsModule,
    RouterLink,
    ButtonComponent,
    CustomSelectComponent,
    FormModalComponent,
  ],
  templateUrl: './course-form.component.html',
  styleUrl: './course-form.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CourseFormComponent {
  protected readonly facade = inject(CoursesFacade);
  protected readonly id = inject(ActivatedRoute).snapshot.paramMap.get('id');
  protected readonly controlError = getControlError;
  protected readonly programOptions = computed(() =>
    this.facade.programs().map((program) => ({
      value: program.id,
      label: `${program.code} · ${program.name}`,
    })),
  );
  protected readonly professorOptions = computed(() =>
    this.facade.professors().map((professor) => ({
      value: professor.id,
      label: `${professor.firstName} ${professor.lastName}`,
      description: `${professor.assignedCourseCount} curso(s) asignado(s)`,
    })),
  );
  protected readonly form = inject(FormBuilder).nonNullable.group({
    academicProgramId: ['', Validators.required],
    code: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(15)]],
    name: ['', [Validators.required, Validators.maxLength(150)]],
    professorId: '',
  });
  constructor() {
    this.facade.loadLookups();
    if (this.id) this.facade.loadById(this.id);
    effect(() => {
      const course = this.facade.selected();
      if (course?.id === this.id) {
        this.form.reset({
          academicProgramId: course.academicProgramId,
          code: course.code,
          name: course.name,
          professorId: course.professorId ?? '',
        });
        this.form.controls.academicProgramId.disable();
      }
    });
  }
  protected submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    const value = this.form.getRawValue();
    this.facade.save(
      this.id
        ? { courseId: this.id, code: value.code, name: value.name }
        : { academicProgramId: value.academicProgramId, code: value.code, name: value.name },
    );
  }
  protected assign(): void {
    const professorId = this.form.controls.professorId.value;
    if (this.id && professorId) this.facade.assignProfessor(this.id, professorId);
  }
}
