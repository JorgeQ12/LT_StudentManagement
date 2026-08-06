import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Icon, IconName } from '@shared/ui/icon';

import { PageHeader } from '@shared/ui/page-header';

interface AdminArea {
  readonly route: string;
  readonly icon: IconName;
  readonly title: string;
  readonly description: string;
}

@Component({
  selector: 'sm-admin-dashboard-page',
  imports: [Icon, PageHeader, RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './admin-dashboard-page.html',
  styleUrl: './admin-dashboard-page.css',
})
export class AdminDashboardPage {
  protected readonly areas: readonly AdminArea[] = [
    {
      route: '/admin/students',
      icon: 'users',
      title: 'Estudiantes',
      description: 'Crea perfiles, actualiza información y gestiona su estado.',
    },
    {
      route: '/admin/academic-programs',
      icon: 'library',
      title: 'Programas',
      description: 'Organiza la oferta institucional y su disponibilidad.',
    },
    {
      route: '/admin/courses',
      icon: 'book-open',
      title: 'Materias',
      description: 'Gestiona materias, programas y profesores asignados.',
    },
    {
      route: '/admin/professors',
      icon: 'presentation',
      title: 'Profesores',
      description: 'Mantén actualizado el equipo docente y su carga.',
    },
  ];
}
