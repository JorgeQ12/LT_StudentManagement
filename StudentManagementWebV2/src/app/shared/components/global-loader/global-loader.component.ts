import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { GlobalLoadingService } from '../../../core/loading/global-loading.service';

@Component({
  selector: 'app-global-loader',
  templateUrl: './global-loader.component.html',
  styleUrl: './global-loader.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class GlobalLoaderComponent {
  protected readonly loading = inject(GlobalLoadingService).isLoading;
}
