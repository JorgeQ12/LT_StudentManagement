import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { GlobalLoaderComponent } from './shared/components/global-loader/global-loader.component';
import { GlobalModalComponent } from './shared/components/global-modal/global-modal.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, GlobalLoaderComponent, GlobalModalComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class App {}
