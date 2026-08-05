import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';

import { API_BASE_URL } from '@core/api/api.config';
import {
  AcademicProgram,
  AcademicProgramId,
  AcademicProgramRequest,
  AdminUpdateStudentRequest,
  AssignProfessorRequest,
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
  UpdateCourseRequest,
} from '@core/api/api.models';
import { API_ROUTES } from '@core/api/api.routes';
import { paginationQueryParams } from '@core/api/http-params';

import {
  AcademicProgramDto,
  CourseDto,
  mapAcademicProgram,
  mapCourse,
  mapPagedResponse,
  mapProfessor,
  mapStudent,
  PagedResponseDto,
  ProfessorDto,
  StudentDto,
} from './administration.dto';

@Injectable({ providedIn: 'root' })
export class AdministrationApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = inject(API_BASE_URL);

  getStudents(query: PageQuery): Observable<PagedResponse<Student>> {
    return this.http
      .get<PagedResponseDto<StudentDto>>(this.url(API_ROUTES.administration.students.all), {
        params: paginationQueryParams(query),
      })
      .pipe(map((page) => mapPagedResponse(page, mapStudent)));
  }

  getStudent(id: StudentId): Observable<Student> {
    return this.http
      .get<StudentDto>(this.url(API_ROUTES.administration.students.byId), {
        params: { studentId: id },
      })
      .pipe(map(mapStudent));
  }

  createStudent(request: RegisterStudentRequest): Observable<Student> {
    return this.http
      .post<StudentDto>(this.url(API_ROUTES.administration.students.create), request)
      .pipe(map(mapStudent));
  }

  updateStudent(id: StudentId, request: AdminUpdateStudentRequest): Observable<Student> {
    return this.http
      .put<StudentDto>(this.url(API_ROUTES.administration.students.update), {
        studentId: id,
        ...request,
      })
      .pipe(map(mapStudent));
  }

  activateStudent(id: StudentId): Observable<Student> {
    return this.http
      .put<StudentDto>(this.url(API_ROUTES.administration.students.activate), { studentId: id })
      .pipe(map(mapStudent));
  }

  deactivateStudent(id: StudentId): Observable<void> {
    return this.http.delete<void>(this.url(API_ROUTES.administration.students.deactivate), {
      params: { studentId: id },
    });
  }

  getAcademicPrograms(): Observable<readonly AcademicProgram[]> {
    return this.http
      .get<readonly AcademicProgramDto[]>(this.url(API_ROUTES.administration.academicPrograms.all))
      .pipe(map((programs) => programs.map(mapAcademicProgram)));
  }

  getAcademicProgram(id: AcademicProgramId): Observable<AcademicProgram> {
    return this.http
      .get<AcademicProgramDto>(this.url(API_ROUTES.administration.academicPrograms.byId), {
        params: { academicProgramId: id },
      })
      .pipe(map(mapAcademicProgram));
  }

  createAcademicProgram(request: AcademicProgramRequest): Observable<AcademicProgram> {
    return this.http
      .post<AcademicProgramDto>(
        this.url(API_ROUTES.administration.academicPrograms.create),
        request,
      )
      .pipe(map(mapAcademicProgram));
  }

  updateAcademicProgram(
    id: AcademicProgramId,
    request: AcademicProgramRequest,
  ): Observable<AcademicProgram> {
    return this.http
      .put<AcademicProgramDto>(this.url(API_ROUTES.administration.academicPrograms.update), {
        academicProgramId: id,
        ...request,
      })
      .pipe(map(mapAcademicProgram));
  }

  activateAcademicProgram(id: AcademicProgramId): Observable<AcademicProgram> {
    return this.http
      .put<AcademicProgramDto>(this.url(API_ROUTES.administration.academicPrograms.activate), {
        academicProgramId: id,
      })
      .pipe(map(mapAcademicProgram));
  }

  deactivateAcademicProgram(id: AcademicProgramId): Observable<void> {
    return this.http.delete<void>(this.url(API_ROUTES.administration.academicPrograms.deactivate), {
      params: { academicProgramId: id },
    });
  }

  getCourses(): Observable<readonly Course[]> {
    return this.http
      .get<readonly CourseDto[]>(this.url(API_ROUTES.administration.courses.all))
      .pipe(map((courses) => courses.map(mapCourse)));
  }

  getCourse(id: CourseId): Observable<Course> {
    return this.http
      .get<CourseDto>(this.url(API_ROUTES.administration.courses.byId), {
        params: { courseId: id },
      })
      .pipe(map(mapCourse));
  }

  createCourse(request: CreateCourseRequest): Observable<Course> {
    return this.http
      .post<CourseDto>(this.url(API_ROUTES.administration.courses.create), request)
      .pipe(map(mapCourse));
  }

  updateCourse(id: CourseId, request: UpdateCourseRequest): Observable<Course> {
    return this.http
      .put<CourseDto>(this.url(API_ROUTES.administration.courses.update), {
        courseId: id,
        ...request,
      })
      .pipe(map(mapCourse));
  }

  activateCourse(id: CourseId): Observable<Course> {
    return this.http
      .put<CourseDto>(this.url(API_ROUTES.administration.courses.activate), { courseId: id })
      .pipe(map(mapCourse));
  }

  deactivateCourse(id: CourseId): Observable<void> {
    return this.http.delete<void>(this.url(API_ROUTES.administration.courses.deactivate), {
      params: { courseId: id },
    });
  }

  assignProfessor(id: CourseId, request: AssignProfessorRequest): Observable<Course> {
    return this.http
      .put<CourseDto>(this.url(API_ROUTES.administration.courses.assignProfessor), {
        courseId: id,
        ...request,
      })
      .pipe(map(mapCourse));
  }

  getProfessors(): Observable<readonly Professor[]> {
    return this.http
      .get<readonly ProfessorDto[]>(this.url(API_ROUTES.administration.professors.all))
      .pipe(map((professors) => professors.map(mapProfessor)));
  }

  getProfessor(id: ProfessorId): Observable<Professor> {
    return this.http
      .get<ProfessorDto>(this.url(API_ROUTES.administration.professors.byId), {
        params: { professorId: id },
      })
      .pipe(map(mapProfessor));
  }

  createProfessor(request: ProfessorRequest): Observable<Professor> {
    return this.http
      .post<ProfessorDto>(this.url(API_ROUTES.administration.professors.create), request)
      .pipe(map(mapProfessor));
  }

  updateProfessor(id: ProfessorId, request: ProfessorRequest): Observable<Professor> {
    return this.http
      .put<ProfessorDto>(this.url(API_ROUTES.administration.professors.update), {
        professorId: id,
        ...request,
      })
      .pipe(map(mapProfessor));
  }

  activateProfessor(id: ProfessorId): Observable<Professor> {
    return this.http
      .put<ProfessorDto>(this.url(API_ROUTES.administration.professors.activate), {
        professorId: id,
      })
      .pipe(map(mapProfessor));
  }

  deactivateProfessor(id: ProfessorId): Observable<void> {
    return this.http.delete<void>(this.url(API_ROUTES.administration.professors.deactivate), {
      params: { professorId: id },
    });
  }

  private url(path: string): string {
    return `${this.baseUrl}/${path}`;
  }
}
