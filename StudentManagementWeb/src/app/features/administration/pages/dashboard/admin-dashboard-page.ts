import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NgIcon } from '@ng-icons/core';

import { PageHeader } from '@shared/ui/page-header';

@Component({
  selector: 'sm-admin-dashboard-page',
  imports: [NgIcon, PageHeader, RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './admin-dashboard-page.html',
  styleUrl: './admin-dashboard-page.css',
})
export class AdminDashboardPage {
  protected readonly areas = [
    {
      route: '/admin/students',
      icon: 'lucideUsersRound',
      title: 'Estudiantes',
      description: 'Crea perfiles, actualiza información y gestiona su estado.',
    },
    {
      route: '/admin/academic-programs',
      icon: 'lucideLibrary',
      title: 'Programas',
      description: 'Organiza la oferta institucional y su disponibilidad.',
    },
    {
      route: '/admin/courses',
      icon: 'lucideBookOpen',
      title: 'Materias',
      description: 'Gestiona materias, programas y profesores asignados.',
    },
    {
      route: '/admin/professors',
      icon: 'lucidePresentation',
      title: 'Profesores',
      description: 'Mantén actualizado el equipo docente y su carga.',
    },
  ] as const;
}
