import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

import { buildIconSpriteDefs } from './icons';

@Component({
  selector: 'sm-icon-sprite',
  imports: [],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `<svg
    width="0"
    height="0"
    aria-hidden="true"
    class="sm-icon-sprite"
    [innerHTML]="defs"
  ></svg>`,
  styles: `
    :host {
      position: absolute;
      width: 0;
      height: 0;
      overflow: hidden;
    }
  `,
})
export class IconSprite {
  private readonly sanitizer = inject(DomSanitizer);

  protected readonly defs: SafeHtml = this.sanitizer.bypassSecurityTrustHtml(
    `<defs>${buildIconSpriteDefs()}</defs>`,
  );
}
