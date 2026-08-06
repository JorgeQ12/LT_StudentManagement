import { AccountRole } from '@core/api/api.models';
import { IconName } from '@shared/ui/icon';

export interface NavigationItem {
  readonly label: string;
  readonly route: string;
  readonly icon: IconName;
  readonly exact?: boolean;
}

export const STUDENT_NAVIGATION: readonly NavigationItem[] = [
  { label: 'Inicio', route: '/student', icon: 'layout-dashboard', exact: true },
  { label: 'Mi perfil', route: '/student/profile', icon: 'user-round' },
  { label: 'Mi inscripción', route: '/student/enrollment', icon: 'book-check' },
  { label: 'Compañeros', route: '/student/enrollment/classmates', icon: 'users' },
];

export const ADMIN_NAVIGATION: readonly NavigationItem[] = [
  { label: 'Inicio', route: '/admin', icon: 'layout-dashboard', exact: true },
  { label: 'Estudiantes', route: '/admin/students', icon: 'users' },
  { label: 'Programas', route: '/admin/academic-programs', icon: 'library' },
  { label: 'Materias', route: '/admin/courses', icon: 'book-open' },
  { label: 'Profesores', route: '/admin/professors', icon: 'presentation' },
];

export function navigationForRole(role: AccountRole): readonly NavigationItem[] {
  return role === 'Administrator' ? ADMIN_NAVIGATION : STUDENT_NAVIGATION;
}

export function homeRouteForRole(role: AccountRole): string {
  return role === 'Administrator' ? '/admin' : '/student';
}
