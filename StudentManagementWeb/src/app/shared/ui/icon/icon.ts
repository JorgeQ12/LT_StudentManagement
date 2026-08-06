import { ChangeDetectionStrategy, Component, computed, input } from '@angular/core';

import { IconName } from './icons';

@Component({
  selector: 'sm-icon',
  imports: [],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <svg
      class="sm-icon__svg"
      [attr.width]="size()"
      [attr.height]="size()"
      [class.spinning]="spin()"
      viewBox="0 0 24 24"
      fill="none"
      focusable="false"
      aria-hidden="true"
    >
      <use [attr.href]="useHref()"></use>
    </svg>
  `,
  styles: `
    :host {
      display: inline-flex;
      flex-shrink: 0;
      line-height: 0;
      vertical-align: middle;
    }

    .sm-icon__svg {
      display: block;
      stroke: currentColor;
    }

    .sm-icon__svg.spinning {
      animation: spin 0.9s linear infinite;
    }

    @keyframes spin {
      to {
        transform: rotate(360deg);
      }
    }
  `,
})
export class Icon {
  readonly name = input.required<IconName>();
  readonly size = input<number | string>(20);
  readonly spin = input(false);

  protected readonly useHref = computed(() => `#icon-${this.name()}`);
}
