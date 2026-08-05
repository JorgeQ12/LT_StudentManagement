import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { NgIcon } from '@ng-icons/core';

import { AuthFacade } from '@core/auth/auth.facade';

interface NavigationItem {
  readonly label: string;
  readonly route: string;
  readonly icon: string;
  readonly exact?: boolean;
}

const STUDENT_NAVIGATION: readonly NavigationItem[] = [
  { label: 'Inicio', route: '/student', icon: 'lucideLayoutDashboard', exact: true },
  { label: 'Mi perfil', route: '/student/profile', icon: 'lucideUserRound' },
  { label: 'Mi inscripción', route: '/student/enrollment', icon: 'lucideBookCheck' },
  { label: 'Compañeros', route: '/student/enrollment/classmates', icon: 'lucideUsersRound' },
];

const ADMIN_NAVIGATION: readonly NavigationItem[] = [
  { label: 'Inicio', route: '/admin', icon: 'lucideLayoutDashboard', exact: true },
  { label: 'Estudiantes', route: '/admin/students', icon: 'lucideUsersRound' },
  { label: 'Programas', route: '/admin/academic-programs', icon: 'lucideLibrary' },
  { label: 'Materias', route: '/admin/courses', icon: 'lucideBookOpen' },
  { label: 'Profesores', route: '/admin/professors', icon: 'lucidePresentation' },
];

@Component({
  selector: 'sm-app-shell',
  imports: [NgIcon, RouterLink, RouterLinkActive, RouterOutlet],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './app-shell.html',
  styleUrl: './app-shell.css',
})
export class AppShell {
  private readonly authFacade = inject(AuthFacade);

  protected readonly mobileNavigationOpen = signal(false);
  protected readonly loggingOut = signal(false);
  protected readonly user = this.authFacade.user;
  protected readonly isAdministrator = computed(() => this.user()?.role === 'Administrator');
  protected readonly navigation = computed(() =>
    this.isAdministrator() ? ADMIN_NAVIGATION : STUDENT_NAVIGATION,
  );
  protected readonly homeRoute = computed(() => (this.isAdministrator() ? '/admin' : '/student'));
  protected readonly initials = computed(
    () => this.user()?.email.slice(0, 2).toUpperCase() ?? 'PA',
  );

  protected closeNavigation(): void {
    this.mobileNavigationOpen.set(false);
  }

  protected async logout(): Promise<void> {
    this.loggingOut.set(true);
    try {
      await this.authFacade.logout();
    } finally {
      this.loggingOut.set(false);
    }
  }
}
