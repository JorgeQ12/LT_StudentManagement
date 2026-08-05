import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NgIcon } from '@ng-icons/core';

@Component({
  selector: 'sm-auth-layout',
  imports: [NgIcon, RouterOutlet],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './auth-layout.html',
  styleUrl: './auth-layout.css',
})
export class AuthLayout {
  protected readonly benefits = [
    { icon: 'lucideShieldCheck', label: 'Acceso protegido' },
    { icon: 'lucideBookOpen', label: 'Materias organizadas' },
    { icon: 'lucideUsersRound', label: 'Comunidad conectada' },
  ] as const;
}
