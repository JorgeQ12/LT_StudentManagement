import { inject, Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';

import {
  AcademicProgram,
  Course,
  CourseId,
  CreateCourseRequest,
  PageQuery,
  Professor,
  ProfessorId,
} from '@core/api/api.models';
import { PagedListStore } from '@core/data/paged-list-store';
import { AcademicProgramsApiService } from '@feature-contracts/academic-programs';
import { ProfessorsApiService } from '@feature-contracts/professors';

import { CoursesApiService } from '../services/courses-api.service';

export interface CourseFormDependencies {
  readonly course: Course | null;
  readonly programs: readonly AcademicProgram[];
  readonly professors: readonly Professor[];
}

@Injectable()
export class CoursesFacade {
  private readonly api = inject(CoursesApiService);
  private readonly programsApi = inject(AcademicProgramsApiService);
  private readonly professorsApi = inject(ProfessorsApiService);
  private readonly store = new PagedListStore<Course>((query) => this.api.list(query));

  readonly items = this.store.items;
  readonly totalCount = this.store.totalCount;
  readonly loading = this.store.loading;
  readonly error = this.store.error;
  readonly busyId = this.store.busyId;

  load(query: PageQuery): Promise<void> {
    return this.store.load(query);
  }

  load$(query: PageQuery) {
    return this.store.load$(query);
  }

  async changeStatus(item: Course, activate: boolean): Promise<void> {
    this.store.setBusy(item.id);
    try {
      if (activate) {
        await firstValueFrom(this.api.activateCourse(item.id));
      } else {
        await firstValueFrom(this.api.deactivateCourse(item.id));
      }
    } finally {
      this.store.setBusy(null);
    }
  }

  async loadCourseForm(id: CourseId | null): Promise<CourseFormDependencies> {
    const [programs, professors, course] = await Promise.all([
      firstValueFrom(this.programsApi.getAll()),
      firstValueFrom(this.professorsApi.getAll()),
      id ? firstValueFrom(this.api.getCourse(id)) : Promise.resolve(null),
    ]);

    return {
      course,
      programs,
      professors: professors.filter((professor) => professor.status === 'Active'),
    };
  }

  async saveCourse(
    id: CourseId | null,
    request: CreateCourseRequest,
    professorId: ProfessorId | null,
    originalProfessorId: ProfessorId | null,
  ): Promise<Course> {
    const course = id
      ? await firstValueFrom(this.api.updateCourse(id, { code: request.code, name: request.name }))
      : await firstValueFrom(this.api.createCourse(request));

    if (professorId && professorId !== originalProfessorId) {
      return firstValueFrom(this.api.assignProfessor(course.id, { professorId }));
    }
    return course;
  }
}
