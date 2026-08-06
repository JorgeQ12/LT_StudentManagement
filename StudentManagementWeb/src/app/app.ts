import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { ToastViewport } from '@shared/ui/toast';
import { IconSprite } from '@shared/ui/icon';

@Component({
  selector: 'sm-root',
  imports: [RouterOutlet, ToastViewport, IconSprite],
  templateUrl: './app.html',
  styleUrl: './app.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class App {}
