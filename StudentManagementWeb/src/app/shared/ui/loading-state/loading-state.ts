import { ChangeDetectionStrategy, Component, input } from '@angular/core';
import { NgIcon } from '@ng-icons/core';

@Component({
  selector: 'sm-loading-state',
  imports: [NgIcon],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './loading-state.html',
  styleUrl: './loading-state.css',
})
export class LoadingState {
  readonly label = input('Cargando información…');
}
