import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { Icon } from '@shared/ui/icon';

import { AuthFacade } from '@core/auth/auth.facade';
import { homeRouteForRole, navigationForRole } from '@core/navigation/app-navigation';

@Component({
  selector: 'sm-app-shell',
  imports: [Icon, RouterLink, RouterLinkActive, RouterOutlet],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './app-shell.html',
  styleUrl: './app-shell.css',
})
export class AppShell {
  private readonly authFacade = inject(AuthFacade);

  protected readonly mobileNavigationOpen = signal(false);
  protected readonly loggingOut = signal(false);
  protected readonly user = this.authFacade.user;
  protected readonly role = computed(() => this.user()?.role ?? 'Student');
  protected readonly navigation = computed(() => navigationForRole(this.role()));
  protected readonly homeRoute = computed(() => homeRouteForRole(this.role()));
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
