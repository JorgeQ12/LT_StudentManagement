import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Icon, IconName } from '@shared/ui/icon';

interface Benefit {
  readonly icon: IconName;
  readonly label: string;
}

@Component({
  selector: 'sm-auth-layout',
  imports: [Icon, RouterOutlet],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './auth-layout.html',
  styleUrl: './auth-layout.css',
})
export class AuthLayout {
  protected readonly benefits: readonly Benefit[] = [
    { icon: 'shield-check', label: 'Acceso protegido' },
    { icon: 'book-open', label: 'Materias organizadas' },
    { icon: 'users', label: 'Comunidad conectada' },
  ];
}
