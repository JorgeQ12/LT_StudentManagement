import { ChangeDetectionStrategy, Component, input } from '@angular/core';
import { AppIconName, IconComponent } from '../icon/icon.component';

@Component({
  selector: 'app-page-header',
  imports: [IconComponent],
  templateUrl: './page-header.component.html',
  styleUrl: './page-header.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PageHeaderComponent {
  readonly eyebrow = input<string | null>(null);
  readonly title = input.required<string>();
  readonly description = input<string | null>(null);
  readonly icon = input<AppIconName | null>(null);
}
