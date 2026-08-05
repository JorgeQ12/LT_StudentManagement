import { ChangeDetectionStrategy, Component } from '@angular/core';
import { NgIcon } from '@ng-icons/core';

@Component({
  selector: 'sm-route-loading-page',
  imports: [NgIcon],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './route-loading-page.html',
  styleUrl: './route-loading-page.css',
})
export class RouteLoadingPage {}
