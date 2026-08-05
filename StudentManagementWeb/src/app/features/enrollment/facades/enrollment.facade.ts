import { computed, inject, Injectable, signal } from '@angular/core';
import { firstValueFrom } from 'rxjs';

import {
  AcademicProgram,
  AcademicProgramId,
  ApiProblemDetails,
  ClassmatesByCourse,
  Course,
  CourseId,
} from '@core/api/api.models';
import { problemFromError, problemMessage } from '@core/api/api-error';
import { AcademicCatalogApiService } from '@feature-contracts/academic-catalog';
import { StudentWorkspaceFacade } from '@feature-contracts/student-profile';

import { canAddCourse, summarizeCourseSelection } from '../models/enrollment-selection';
import { EnrollmentsApiService } from '../services/enrollments-api.service';

export type EnrollmentSaveResult = 'created' | 'replaced' | null;

@Injectable()
export class EnrollmentFacade {
  private readonly catalogApi = inject(AcademicCatalogApiService);
  private readonly enrollmentsApi = inject(EnrollmentsApiService);
  private readonly workspace = inject(StudentWorkspaceFacade);

  private readonly programsState = signal<readonly AcademicProgram[]>([]);
  private readonly coursesState = signal<readonly Course[]>([]);
  private readonly selectedProgramIdState = signal<AcademicProgramId | null>(null);
  private readonly selectedCourseIdsState = signal<readonly CourseId[]>([]);
  private readonly loadingCatalogState = signal(false);
  private readonly savingState = signal(false);
  private readonly replacingState = signal(false);
  private readonly problemState = signal<ApiProblemDetails | null>(null);
  private readonly classmatesState = signal<readonly ClassmatesByCourse[]>([]);
  private readonly classmatesLoadingState = signal(false);
  private readonly classmatesErrorState = signal<string | null>(null);
  private initializeRequest?: Promise<void>;

  readonly profile = this.workspace.profile;
  readonly enrollment = this.workspace.enrollment;
  readonly workspaceLoading = this.workspace.loading;
  readonly workspaceError = this.workspace.error;
  readonly programs = computed(() => this.programsState());
  readonly courses = computed(() => this.coursesState());
  readonly selectedProgramId = computed(() => this.selectedProgramIdState());
  readonly selectedCourseIds = computed(() => this.selectedCourseIdsState());
  readonly loadingCatalog = computed(() => this.loadingCatalogState());
  readonly saving = computed(() => this.savingState());
  readonly replacing = computed(() => this.replacingState());
  readonly problem = computed(() => this.problemState());
  readonly classmates = computed(() => this.classmatesState());
  readonly classmatesLoading = computed(() => this.classmatesLoadingState());
  readonly classmatesError = computed(() => this.classmatesErrorState());

  readonly selectedCourses = computed(() => {
    const ids = new Set(this.selectedCourseIdsState());
    return this.coursesState().filter((course) => ids.has(course.id));
  });
  readonly selectionSummary = computed(() => summarizeCourseSelection(this.selectedCourses()));
  readonly totalCredits = computed(() => this.selectionSummary().totalCredits);
  readonly professorCount = computed(() => this.selectionSummary().professorCount);
  readonly selectionIsValid = computed(
    () => this.selectedProgramIdState() !== null && this.selectionSummary().isValid,
  );

  initialize(force = false): Promise<void> {
    if (force) {
      this.initializeRequest = undefined;
    }
    this.initializeRequest ??= Promise.all([this.workspace.load(force), this.loadPrograms()]).then(
      () => undefined,
    );
    return this.initializeRequest;
  }

  async selectProgram(value: string): Promise<void> {
    const academicProgramId = value ? (value as AcademicProgramId) : null;
    this.selectedProgramIdState.set(academicProgramId);
    this.selectedCourseIdsState.set([]);
    this.coursesState.set([]);
    this.problemState.set(null);
    if (academicProgramId) {
      await this.loadCourses(academicProgramId);
    }
  }

  isSelected(courseId: CourseId): boolean {
    return this.selectedCourseIdsState().includes(courseId);
  }

  isUnavailable(course: Course): boolean {
    return !this.isSelected(course.id) && !canAddCourse(this.selectedCourses(), course);
  }

  toggleCourse(course: Course): boolean {
    if (this.isSelected(course.id)) {
      this.selectedCourseIdsState.update((ids) => ids.filter((id) => id !== course.id));
      return true;
    }
    if (this.isUnavailable(course)) {
      return false;
    }
    this.selectedCourseIdsState.update((ids) => [...ids, course.id]);
    return true;
  }

  async startReplacement(): Promise<void> {
    const enrollment = this.workspace.enrollment();
    if (!enrollment) {
      return;
    }
    this.replacingState.set(true);
    this.selectedProgramIdState.set(enrollment.academicProgram.id);
    await this.loadCourses(enrollment.academicProgram.id);
    this.selectedCourseIdsState.set(enrollment.courses.map((course) => course.id));
  }

  stopReplacement(): void {
    this.replacingState.set(false);
    this.resetSelection();
  }

  async saveEnrollment(): Promise<EnrollmentSaveResult> {
    const academicProgramId = this.selectedProgramIdState();
    if (!academicProgramId || !this.selectionIsValid()) {
      return null;
    }

    this.savingState.set(true);
    this.problemState.set(null);
    try {
      if (this.replacingState()) {
        await this.workspace.replaceCourses({ courseIds: this.selectedCourseIdsState() });
        this.replacingState.set(false);
        this.resetSelection();
        return 'replaced';
      }

      await this.workspace.createEnrollment({
        academicProgramId,
        courseIds: this.selectedCourseIdsState(),
      });
      this.resetSelection();
      return 'created';
    } catch (error) {
      this.problemState.set(problemFromError(error));
      return null;
    } finally {
      this.savingState.set(false);
    }
  }

  async cancelEnrollment(): Promise<boolean> {
    this.savingState.set(true);
    this.problemState.set(null);
    try {
      await this.workspace.cancelEnrollment();
      this.replacingState.set(false);
      this.resetSelection();
      return true;
    } catch (error) {
      this.problemState.set(problemFromError(error));
      return false;
    } finally {
      this.savingState.set(false);
    }
  }

  async loadClassmates(): Promise<void> {
    await this.workspace.load();
    if (!this.workspace.enrollment()) {
      this.classmatesState.set([]);
      this.classmatesLoadingState.set(false);
      return;
    }

    this.classmatesLoadingState.set(true);
    this.classmatesErrorState.set(null);
    try {
      this.classmatesState.set(await firstValueFrom(this.enrollmentsApi.getClassmates()));
    } catch (error) {
      this.classmatesErrorState.set(problemMessage(error));
    } finally {
      this.classmatesLoadingState.set(false);
    }
  }

  private async loadPrograms(): Promise<void> {
    this.loadingCatalogState.set(true);
    this.problemState.set(null);
    try {
      this.programsState.set(await firstValueFrom(this.catalogApi.getActivePrograms()));
    } catch (error) {
      this.problemState.set(problemFromError(error));
    } finally {
      this.loadingCatalogState.set(false);
    }
  }

  private async loadCourses(academicProgramId: AcademicProgramId): Promise<void> {
    this.loadingCatalogState.set(true);
    this.problemState.set(null);
    try {
      this.coursesState.set(
        await firstValueFrom(this.catalogApi.getActiveCourses(academicProgramId)),
      );
    } catch (error) {
      this.problemState.set(problemFromError(error));
    } finally {
      this.loadingCatalogState.set(false);
    }
  }

  private resetSelection(): void {
    this.selectedProgramIdState.set(null);
    this.selectedCourseIdsState.set([]);
    this.coursesState.set([]);
    this.problemState.set(null);
  }
}
