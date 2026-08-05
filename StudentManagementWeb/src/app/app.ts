import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { ToastViewport } from '@shared/ui/toast';

@Component({
  selector: 'sm-root',
  imports: [RouterOutlet, ToastViewport],
  templateUrl: './app.html',
  styleUrl: './app.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class App {}
