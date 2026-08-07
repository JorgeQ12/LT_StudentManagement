import { ChangeDetectionStrategy, Component, computed, input } from '@angular/core';

@Component({
  selector: 'app-status-badge',
  templateUrl: './status-badge.component.html',
  styleUrl: './status-badge.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class StatusBadgeComponent {
  readonly status = input.required<'Active' | 'Inactive' | 'Cancelled'>();
  protected readonly label = computed(
    () => ({ Active: 'Activo', Inactive: 'Inactivo', Cancelled: 'Cancelado' })[this.status()],
  );
}
