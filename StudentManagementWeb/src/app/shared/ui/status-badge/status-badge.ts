import { ChangeDetectionStrategy, Component, computed, input } from '@angular/core';

type Status = 'Active' | 'Inactive' | 'Cancelled';

@Component({
  selector: 'sm-status-badge',
  changeDetection: ChangeDetectionStrategy.OnPush,
  host: { class: 'inline-flex' },
  templateUrl: './status-badge.html',
  styleUrl: './status-badge.css',
})
export class StatusBadge {
  readonly status = input.required<Status>();
  protected readonly label = computed(() => {
    const labels: Record<Status, string> = {
      Active: 'Activo',
      Inactive: 'Inactivo',
      Cancelled: 'Cancelada',
    };
    return labels[this.status()];
  });
}
