import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthFacade } from '../../core/auth/auth.facade';
import { AppIconName, IconComponent } from '../../shared/components/icon/icon.component';
import { ModalService } from '../../shared/services/modal/modal.service';

interface NavigationItem {
  readonly label: string;
  readonly route: string;
  readonly icon: AppIconName;
}

@Component({
  selector: 'app-shell',
  imports: [RouterOutlet, RouterLink, RouterLinkActive, IconComponent],
  templateUrl: './app-shell.component.html',
  styleUrl: './app-shell.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppShellComponent {
  private readonly auth = inject(AuthFacade);
  private readonly router = inject(Router);
  private readonly modal = inject(ModalService);

  protected readonly user = this.auth.user;
  protected readonly menuOpen = signal(false);
  protected readonly navigation = computed<readonly NavigationItem[]>(() =>
    this.auth.isAdministrator()
      ? [
          { label: 'Resumen', route: '/admin', icon: 'dashboard' },
          { label: 'Programas', route: '/admin/programs', icon: 'programs' },
          { label: 'Cursos', route: '/admin/courses', icon: 'courses' },
          { label: 'Profesores', route: '/admin/professors', icon: 'professors' },
          { label: 'Estudiantes', route: '/admin/students', icon: 'students' },
        ]
      : [
          { label: 'Inicio', route: '/student', icon: 'dashboard' },
          { label: 'Matrícula', route: '/student/enrollment', icon: 'enrollment' },
          { label: 'Mi perfil', route: '/student/profile', icon: 'profile' },
        ],
  );

  protected toggleMenu(): void {
    this.menuOpen.update((open) => !open);
  }

  protected closeMenu(): void {
    this.menuOpen.set(false);
  }

  protected async logout(): Promise<void> {
    const confirmed = await this.modal.confirm({
      title: 'Cerrar sesión',
      message: '¿Quieres salir del portal académico?',
      confirmText: 'Cerrar sesión',
      cancelText: 'Continuar aquí',
    });
    if (!confirmed) return;

    this.auth.logout().subscribe({
      next: () => void this.router.navigate(['/auth/login']),
      error: () =>
        void this.modal.error({
          title: 'No fue posible cerrar la sesión',
          message: 'Inténtalo nuevamente. Si el problema continúa, cierra el navegador.',
        }),
    });
  }
}
