import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ActivatedRoute, Router, RouterOutlet } from '@angular/router';
import { AuthFacade } from '../../../../core/auth/auth.facade';

@Component({
  selector: 'app-auth-layout-page',
  imports: [RouterOutlet],
  templateUrl: './auth-layout.page.html',
  styleUrl: './auth-layout.page.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AuthLayoutPage {
  private readonly auth = inject(AuthFacade);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  constructor() {
    // El formulario se renderiza de inmediato. En segundo plano comprobamos si ya
    // existe una sesión activa para llevar al usuario a su panel sin bloquear la pantalla.
    this.auth
      .ensureSession()
      .pipe(takeUntilDestroyed())
      .subscribe((user) => {
        if (!user) return;
        const returnUrl = this.route.snapshot.queryParamMap.get('returnUrl');
        void this.router.navigateByUrl(
          returnUrl ?? (user.role === 'Administrator' ? '/admin' : '/student'),
        );
      });
  }
}
