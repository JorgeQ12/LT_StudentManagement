import {
  ChangeDetectionStrategy,
  Component,
  computed,
  effect,
  inject,
  signal,
} from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { CustomSelectComponent } from '../../../../shared/components/custom-select/custom-select.component';
import { EmptyStateComponent } from '../../../../shared/components/empty-state/empty-state.component';
import { IconComponent } from '../../../../shared/components/icon/icon.component';
import { PageHeaderComponent } from '../../../../shared/components/page-header/page-header.component';
import { CatalogCourse } from '../../../academic-catalog/public-api';
import { EnrollmentFacade } from '../../facade/enrollment.facade';
@Component({
  selector: 'app-enrollment-page',
  imports: [
    ReactiveFormsModule,
    ButtonComponent,
    CustomSelectComponent,
    EmptyStateComponent,
    IconComponent,
    PageHeaderComponent,
  ],
  templateUrl: './enrollment.page.html',
  styleUrl: './enrollment.page.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EnrollmentPage {
  protected readonly facade = inject(EnrollmentFacade);
  private readonly selectedIds = signal<readonly string[]>([]);
  protected readonly selectedCourseIds = this.selectedIds.asReadonly();
  protected readonly programOptions = computed(() =>
    this.facade.programs().map((program) => ({
      value: program.id,
      label: `${program.code} · ${program.name}`,
    })),
  );
  protected readonly form = inject(FormBuilder).nonNullable.group({
    academicProgramId: ['', Validators.required],
  });
  constructor() {
    this.facade.load();
    effect(() => {
      const enrollment = this.facade.enrollment();
      if (enrollment) {
        this.form.controls.academicProgramId.setValue(enrollment.academicProgram.id, {
          emitEvent: false,
        });
        this.form.controls.academicProgramId.disable({ emitEvent: false });
        this.selectedIds.set(enrollment.courses.map((course) => course.id));
      } else {
        this.form.controls.academicProgramId.enable({ emitEvent: false });
        this.form.controls.academicProgramId.setValue('', { emitEvent: false });
        this.selectedIds.set([]);
      }
    });
    this.form.controls.academicProgramId.valueChanges.subscribe((programId) => {
      this.selectedIds.set([]);
      this.facade.loadCourses(programId);
    });
  }
  protected isSelected(courseId: string): boolean {
    return this.selectedIds().includes(courseId);
  }
  protected toggleCourse(course: CatalogCourse): void {
    if (!course.professorId) return;
    this.selectedIds.update((ids) =>
      ids.includes(course.id)
        ? ids.filter((id) => id !== course.id)
        : ids.length < 3
          ? [...ids, course.id]
          : ids,
    );
  }
  protected save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.facade.save(this.form.getRawValue().academicProgramId, this.selectedIds());
  }
}
