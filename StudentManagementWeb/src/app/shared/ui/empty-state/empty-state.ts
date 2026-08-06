import { ChangeDetectionStrategy, Component, input } from '@angular/core';
import { Icon, IconName } from '@shared/ui/icon';

@Component({
  selector: 'sm-empty-state',
  imports: [Icon],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './empty-state.html',
  styleUrl: './empty-state.css',
})
export class EmptyState {
  readonly icon = input<IconName>('inbox');
  readonly title = input.required<string>();
  readonly description = input.required<string>();
}
