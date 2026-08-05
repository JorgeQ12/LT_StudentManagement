import { Course } from '@core/api/api.models';

export interface CourseSelectionSummary {
  readonly courseCount: number;
  readonly totalCredits: number;
  readonly professorCount: number;
  readonly isValid: boolean;
}

export function summarizeCourseSelection(courses: readonly Course[]): CourseSelectionSummary {
  const totalCredits = courses.reduce((total, course) => total + course.credits, 0);
  const professorCount = new Set(
    courses.map((course) => course.professorId).filter((id) => id !== null),
  ).size;

  return {
    courseCount: courses.length,
    totalCredits,
    professorCount,
    isValid: courses.length === 3 && totalCredits === 9 && professorCount === 3,
  };
}

export function canAddCourse(selectedCourses: readonly Course[], candidate: Course): boolean {
  if (selectedCourses.some((course) => course.id === candidate.id)) {
    return true;
  }
  if (selectedCourses.length >= 3 || candidate.professorId === null) {
    return false;
  }

  return !selectedCourses.some((course) => course.professorId === candidate.professorId);
}
