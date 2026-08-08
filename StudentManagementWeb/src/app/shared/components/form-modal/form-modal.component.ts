import { A11yModule } from '@angular/cdk/a11y';
import { ChangeDetectionStrategy, Component, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AppIconName, IconComponent } from '../icon/icon.component';

@Component({
  selector: 'app-form-modal',
  imports: [A11yModule, RouterLink, IconComponent],
  templateUrl: './form-modal.component.html',
  styleUrl: './form-modal.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FormModalComponent {
  readonly title = input.required<string>();
  readonly closeLink = input.required<string>();
  readonly icon = input.required<AppIconName>();
}
