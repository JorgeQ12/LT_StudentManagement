import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { Icon } from '@shared/ui/icon';

import { ToastService } from './toast.service';

@Component({
  selector: 'sm-toast-viewport',
  imports: [Icon],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './toast-viewport.html',
  styleUrl: './toast-viewport.css',
})
export class ToastViewport {
  protected readonly toastService = inject(ToastService);
}
