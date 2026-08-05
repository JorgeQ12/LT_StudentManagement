import { HttpErrorResponse } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { firstValueFrom } from 'rxjs';

import {
  Enrollment,
  EnrollmentRequest,
  ReplaceEnrollmentCoursesRequest,
  Student,
  UpdateStudentProfileRequest,
} from '@core/api/api.models';
import { problemMessage } from '@core/api/api-error';
import { EnrollmentsApiService } from '@feature-contracts/enrollment';

import { StudentProfilesApiService } from '../services/student-profiles-api.service';

@Injectable()
export class StudentWorkspaceFacade {
  private readonly profilesApi = inject(StudentProfilesApiService);
  private readonly enrollmentsApi = inject(EnrollmentsApiService);
  private readonly profileState = signal<Student | null>(null);
  private readonly enrollmentState = signal<Enrollment | null>(null);
  private readonly loadingState = signal(false);
  private readonly errorState = signal<string | null>(null);
  private loadRequest?: Promise<void>;

  readonly profile = computed(() => this.profileState());
  readonly enrollment = computed(() => this.enrollmentState());
  readonly loading = computed(() => this.loadingState());
  readonly error = computed(() => this.errorState());
  readonly fullName = computed(() => {
    const profile = this.profileState();
    return profile ? `${profile.firstName} ${profile.lastName}` : '';
  });

  load(force = false): Promise<void> {
    if (force) {
      this.loadRequest = undefined;
    }
    if (!this.loadRequest) {
      this.loadingState.set(true);
      this.errorState.set(null);
      this.loadRequest = Promise.all([
        firstValueFrom(this.profilesApi.getCurrent()),
        firstValueFrom(this.enrollmentsApi.getCurrent()).catch((error: unknown) => {
          if (error instanceof HttpErrorResponse && error.status === 404) {
            return null;
          }
          throw error;
        }),
      ])
        .then(([profile, enrollment]) => {
          this.profileState.set(profile);
          this.enrollmentState.set(enrollment);
        })
        .catch((error: unknown) => {
          this.errorState.set(problemMessage(error));
        })
        .finally(() => this.loadingState.set(false));
    }

    return this.loadRequest;
  }

  async updateProfile(request: UpdateStudentProfileRequest): Promise<Student> {
    const profile = await firstValueFrom(this.profilesApi.updateCurrent(request));
    this.profileState.set(profile);
    return profile;
  }

  async deactivateAccount(): Promise<void> {
    await firstValueFrom(this.profilesApi.deactivateCurrent());
    this.profileState.set(null);
    this.enrollmentState.set(null);
  }

  async createEnrollment(request: EnrollmentRequest): Promise<Enrollment> {
    const enrollment = await firstValueFrom(this.enrollmentsApi.create(request));
    this.enrollmentState.set(enrollment);
    return enrollment;
  }

  async replaceCourses(request: ReplaceEnrollmentCoursesRequest): Promise<Enrollment> {
    const enrollment = await firstValueFrom(this.enrollmentsApi.replaceSelectedCourses(request));
    this.enrollmentState.set(enrollment);
    return enrollment;
  }

  async cancelEnrollment(): Promise<void> {
    await firstValueFrom(this.enrollmentsApi.cancel());
    this.enrollmentState.set(null);
  }
}
