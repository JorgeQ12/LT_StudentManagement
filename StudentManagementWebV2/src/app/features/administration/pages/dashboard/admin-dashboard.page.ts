import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AppIconName, IconComponent } from '../../../../shared/components/icon/icon.component';
@Component({
  selector: 'app-admin-dashboard-page',
  imports: [RouterLink, IconComponent],
  templateUrl: './admin-dashboard.page.html',
  styleUrl: './admin-dashboard.page.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AdminDashboardPage {
  protected readonly resources = [
    {
      title: 'Programas académicos',
      description: 'Gestiona la oferta académica y sus estados.',
      route: 'programs',
      icon: 'programs' satisfies AppIconName,
    },
    {
      title: 'Cursos',
      description: 'Organiza cursos y asignaciones docentes.',
      route: 'courses',
      icon: 'courses' satisfies AppIconName,
    },
    {
      title: 'Profesores',
      description: 'Administra docentes y su carga de cursos.',
      route: 'professors',
      icon: 'professors' satisfies AppIconName,
    },
    {
      title: 'Estudiantes',
      description: 'Gestiona cuentas e información estudiantil.',
      route: 'students',
      icon: 'students' satisfies AppIconName,
    },
  ] as const;
}
