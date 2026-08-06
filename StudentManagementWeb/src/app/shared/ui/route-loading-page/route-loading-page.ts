import { ChangeDetectionStrategy, Component } from '@angular/core';
import { Icon } from '@shared/ui/icon';

@Component({
  selector: 'sm-route-loading-page',
  imports: [Icon],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './route-loading-page.html',
  styleUrl: './route-loading-page.css',
})
export class RouteLoadingPage {}
