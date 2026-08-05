import { NgTemplateOutlet } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NgIcon } from '@ng-icons/core';
import { firstValueFrom } from 'rxjs';

import { Course, CourseId } from '@core/api/api.models';
import { ConfirmDialogService } from '@shared/ui/confirm-dialog';
import { LoadingState } from '@shared/ui/loading-state';
import { PageHeader } from '@shared/ui/page-header';
import { StatusBadge } from '@shared/ui/status-badge';
import { ToastService } from '@shared/ui/toast';

import { EnrollmentFacade } from '../../facades/enrollment.facade';

@Component({
  selector: 'sm-enrollment-page',
  imports: [LoadingState, NgIcon, NgTemplateOutlet, PageHeader, RouterLink, StatusBadge],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './enrollment-page.html',
  styleUrl: './enrollment-page.css',
})
export class EnrollmentPage {
  private readonly confirmDialog = inject(ConfirmDialogService);
  private readonly toast = inject(ToastService);

  protected readonly facade = inject(EnrollmentFacade);
  protected readonly programs = this.facade.programs;
  protected readonly courses = this.facade.courses;
  protected readonly selectedProgramId = this.facade.selectedProgramId;
  protected readonly loadingCatalog = this.facade.loadingCatalog;
  protected readonly saving = this.facade.saving;
  protected readonly replacing = this.facade.replacing;
  protected readonly problem = this.facade.problem;
  protected readonly selectedCourses = this.facade.selectedCourses;
  protected readonly totalCredits = this.facade.totalCredits;
  protected readonly professorCount = this.facade.professorCount;
  protected readonly selectionIsValid = this.facade.selectionIsValid;

  constructor() {
    void this.facade.initialize();
  }

  protected async selectProgram(value: string): Promise<void> {
    await this.facade.selectProgram(value);
  }

  protected isSelected(courseId: CourseId): boolean {
    return this.facade.isSelected(courseId);
  }

  protected isUnavailable(course: Course): boolean {
    return this.facade.isUnavailable(course);
  }

  protected toggleCourse(course: Course): void {
    if (!this.facade.toggleCourse(course)) {
      this.toast.info('Elige otro profesor', 'Cada materia debe tener un profesor diferente.');
    }
  }

  protected async startReplacement(): Promise<void> {
    await this.facade.startReplacement();
  }

  protected stopReplacement(): void {
    this.facade.stopReplacement();
  }

  protected async saveEnrollment(): Promise<void> {
    const result = await this.facade.saveEnrollment();
    if (result === 'replaced') {
      this.toast.success('Materias actualizadas', 'Tu nueva selección quedó guardada.');
    } else if (result === 'created') {
      this.toast.success('Inscripción creada', 'Ya puedes consultar tus materias y compañeros.');
    }
  }

  protected async cancelEnrollment(): Promise<void> {
    const confirmed = await firstValueFrom(
      this.confirmDialog.open({
        title: '¿Cancelar tu inscripción?',
        message:
          'Las materias seleccionadas dejarán de estar activas. Luego podrás crear una nueva inscripción.',
        confirmLabel: 'Sí, cancelar',
        tone: 'danger',
      }),
    );
    if (!confirmed) {
      return;
    }

    const cancelled = await this.facade.cancelEnrollment();
    if (cancelled) {
      this.toast.success('Inscripción cancelada');
    } else {
      const problem = this.facade.problem();
      if (!problem) {
        return;
      }
      this.toast.error(problem.title, problem.detail);
    }
  }
}
