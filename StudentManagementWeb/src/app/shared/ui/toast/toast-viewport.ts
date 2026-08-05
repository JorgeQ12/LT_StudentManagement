import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { NgIcon } from '@ng-icons/core';

import { ToastService } from './toast.service';

@Component({
  selector: 'sm-toast-viewport',
  imports: [NgIcon],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './toast-viewport.html',
  styleUrl: './toast-viewport.css',
})
export class ToastViewport {
  protected readonly toastService = inject(ToastService);
}
