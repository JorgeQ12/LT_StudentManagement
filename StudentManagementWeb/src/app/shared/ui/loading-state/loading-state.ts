import { ChangeDetectionStrategy, Component, input } from '@angular/core';
import { Icon } from '@shared/ui/icon';

@Component({
  selector: 'sm-loading-state',
  imports: [Icon],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './loading-state.html',
  styleUrl: './loading-state.css',
})
export class LoadingState {
  readonly label = input('Cargando información…');
}
