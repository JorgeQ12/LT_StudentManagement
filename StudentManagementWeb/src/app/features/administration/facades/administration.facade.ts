import { computed, inject, Injectable, signal } from '@angular/core';
import {
  catchError,
  defer,
  finalize,
  firstValueFrom,
  forkJoin,
  map,
  Observable,
  of,
  switchMap,
  tap,
} from 'rxjs';

import {
  AcademicProgram,
  AcademicProgramId,
  AcademicProgramRequest,
  AdminUpdateStudentRequest,
  Course,
  CourseId,
  CreateCourseRequest,
  PageQuery,
  PagedResponse,
  Professor,
  ProfessorId,
  ProfessorRequest,
  RegisterStudentRequest,
  Student,
  StudentId,
} from '@core/api/api.models';
import { problemMessage } from '@core/api/api-error';

import { AdministrationApiService } from '../services/administration-api.service';

export type AdminResource = 'students' | 'academic-programs' | 'courses' | 'professors';
export type AdminEntity = Student | AcademicProgram | Course | Professor;

export interface CourseFormDependencies {
  readonly course: Course | null;
  readonly programs: readonly AcademicProgram[];
  readonly professors: readonly Professor[];
}

@Injectable()
export class AdministrationFacade {
  private readonly api = inject(AdministrationApiService);
  private readonly itemsState = signal<readonly AdminEntity[]>([]);
  private readonly totalCountState = signal(0);
  private readonly loadingState = signal(false);
  private readonly errorState = signal<string | null>(null);
  private readonly busyIdState = signal<string | null>(null);

  readonly items = computed(() => this.itemsState());
  readonly totalCount = computed(() => this.totalCountState());
  readonly loading = computed(() => this.loadingState());
  readonly error = computed(() => this.errorState());
  readonly busyId = computed(() => this.busyIdState());

  load(resource: AdminResource, query: PageQuery): Promise<void> {
    return firstValueFrom(this.load$(resource, query));
  }

  load$(resource: AdminResource, query: PageQuery): Observable<void> {
    return defer(() => {
      this.loadingState.set(true);
      this.errorState.set(null);

      return this.loadPage$(resource, query).pipe(
        tap((page) => {
          this.itemsState.set(page.items);
          this.totalCountState.set(page.totalCount);
        }),
        map(() => undefined),
        catchError((error: unknown) => {
          this.itemsState.set([]);
          this.totalCountState.set(0);
          this.errorState.set(problemMessage(error));
          return of(undefined);
        }),
        finalize(() => this.loadingState.set(false)),
      );
    });
  }

  async changeStatus(resource: AdminResource, item: AdminEntity, activate: boolean): Promise<void> {
    this.busyIdState.set(item.id);
    try {
      switch (resource) {
        case 'students':
          if (activate) {
            await firstValueFrom(this.api.activateStudent(item.id as StudentId));
          } else {
            await firstValueFrom(this.api.deactivateStudent(item.id as StudentId));
          }
          return;
        case 'academic-programs':
          if (activate) {
            await firstValueFrom(this.api.activateAcademicProgram(item.id as AcademicProgramId));
          } else {
            await firstValueFrom(this.api.deactivateAcademicProgram(item.id as AcademicProgramId));
          }
          return;
        case 'courses':
          if (activate) {
            await firstValueFrom(this.api.activateCourse(item.id as CourseId));
          } else {
            await firstValueFrom(this.api.deactivateCourse(item.id as CourseId));
          }
          return;
        case 'professors':
          if (activate) {
            await firstValueFrom(this.api.activateProfessor(item.id as ProfessorId));
          } else {
            await firstValueFrom(this.api.deactivateProfessor(item.id as ProfessorId));
          }
      }
    } finally {
      this.busyIdState.set(null);
    }
  }

  getStudent(id: StudentId): Promise<Student> {
    return firstValueFrom(this.api.getStudent(id));
  }

  createStudent(request: RegisterStudentRequest): Promise<Student> {
    return firstValueFrom(this.api.createStudent(request));
  }

  updateStudent(id: StudentId, request: AdminUpdateStudentRequest): Promise<Student> {
    return firstValueFrom(this.api.updateStudent(id, request));
  }

  getAcademicProgram(id: AcademicProgramId): Promise<AcademicProgram> {
    return firstValueFrom(this.api.getAcademicProgram(id));
  }

  createAcademicProgram(request: AcademicProgramRequest): Promise<AcademicProgram> {
    return firstValueFrom(this.api.createAcademicProgram(request));
  }

  updateAcademicProgram(
    id: AcademicProgramId,
    request: AcademicProgramRequest,
  ): Promise<AcademicProgram> {
    return firstValueFrom(this.api.updateAcademicProgram(id, request));
  }

  getProfessor(id: ProfessorId): Promise<Professor> {
    return firstValueFrom(this.api.getProfessor(id));
  }

  createProfessor(request: ProfessorRequest): Promise<Professor> {
    return firstValueFrom(this.api.createProfessor(request));
  }

  updateProfessor(id: ProfessorId, request: ProfessorRequest): Promise<Professor> {
    return firstValueFrom(this.api.updateProfessor(id, request));
  }

  async loadCourseForm(id: CourseId | null): Promise<CourseFormDependencies> {
    const [programPage, professorPage, course] = await Promise.all([
      firstValueFrom(this.api.getAcademicPrograms()),
      firstValueFrom(this.api.getProfessors()),
      id ? firstValueFrom(this.api.getCourse(id)) : Promise.resolve(null),
    ]);

    return {
      course,
      programs: programPage,
      professors: professorPage.filter((professor) => professor.status === 'Active'),
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

  private loadPage$(
    resource: AdminResource,
    query: PageQuery,
  ): Observable<PagedResponse<AdminEntity>> {
    switch (resource) {
      case 'students':
        return this.loadStudentsPage$(query);
      case 'academic-programs':
        return this.api.getAcademicPrograms().pipe(map((items) => this.toPage(items, query)));
      case 'courses':
        return this.api.getCourses().pipe(map((items) => this.toPage(items, query)));
      case 'professors':
        return this.api.getProfessors().pipe(map((items) => this.toPage(items, query)));
    }
  }

  private loadStudentsPage$(query: PageQuery): Observable<PagedResponse<Student>> {
    if (!query.search?.trim() && !query.status) {
      return this.api.getStudents(query);
    }

    return this.api.getStudents({ pageNumber: 1, pageSize: 100 }).pipe(
      switchMap((firstPage) => {
        const pageCount = Math.ceil(firstPage.totalCount / firstPage.pageSize);
        const remainingRequests = Array.from({ length: Math.max(0, pageCount - 1) }, (_, index) =>
          this.api.getStudents({
            pageNumber: index + 2,
            pageSize: firstPage.pageSize,
          }),
        );

        return (remainingRequests.length ? forkJoin(remainingRequests) : of([])).pipe(
          map((remainingPages) =>
            this.toPage(
              [firstPage, ...remainingPages].flatMap((page) => page.items),
              query,
            ),
          ),
        );
      }),
    );
  }

  private toPage<TItem extends AdminEntity>(
    items: readonly TItem[],
    query: PageQuery,
  ): PagedResponse<TItem> {
    const search = query.search?.trim().toLocaleLowerCase('es-CO') ?? '';
    const filtered = items.filter((item) => {
      if (query.status && item.status !== query.status) {
        return false;
      }
      if (
        query.academicProgramId &&
        'academicProgramId' in item &&
        item.academicProgramId !== query.academicProgramId
      ) {
        return false;
      }
      return !search || this.searchableText(item).includes(search);
    });
    const pageNumber = Math.max(1, query.pageNumber ?? 1);
    const pageSize = Math.max(1, query.pageSize ?? 10);
    const start = (pageNumber - 1) * pageSize;

    return {
      items: filtered.slice(start, start + pageSize),
      pageNumber,
      pageSize,
      totalCount: filtered.length,
    };
  }

  private searchableText(item: AdminEntity): string {
    const values =
      'documentNumber' in item
        ? [item.firstName, item.lastName, item.documentNumber, item.email]
        : 'description' in item
          ? [item.code, item.name, item.description]
          : 'professorFullName' in item
            ? [item.code, item.name, item.professorFullName]
            : [item.firstName, item.lastName];

    return values.filter(Boolean).join(' ').toLocaleLowerCase('es-CO');
  }
}
