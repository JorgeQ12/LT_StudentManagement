import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'app-route-pending',
  template: '<p class="sr-only">Redirigiendo…</p>',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RoutePendingComponent {}
