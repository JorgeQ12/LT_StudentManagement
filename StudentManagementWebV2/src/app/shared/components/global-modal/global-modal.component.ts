import { A11yModule } from '@angular/cdk/a11y';
import { ChangeDetectionStrategy, Component, HostListener, inject } from '@angular/core';
import { ModalService } from '../../services/modal/modal.service';
import { ModalType } from '../../services/modal/modal.types';
import { ButtonComponent } from '../button/button.component';
import { AppIconName, IconComponent } from '../icon/icon.component';

@Component({
  selector: 'app-global-modal',
  imports: [A11yModule, ButtonComponent, IconComponent],
  templateUrl: './global-modal.component.html',
  styleUrl: './global-modal.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class GlobalModalComponent {
  private readonly modal = inject(ModalService);
  protected readonly state = this.modal.state;

  @HostListener('document:keydown.escape')
  onEscape(): void {
    const state = this.state();
    if (state?.dismissible !== false) {
      this.modal.resolve(false);
    }
  }

  protected confirm(): void {
    this.modal.resolve(true);
  }

  protected cancel(): void {
    this.modal.resolve(false);
  }

  protected onBackdrop(event: MouseEvent): void {
    if (event.target === event.currentTarget && this.state()?.dismissible !== false) {
      this.cancel();
    }
  }

  protected iconName(type: ModalType): AppIconName {
    const icons: Record<ModalType, AppIconName> = {
      success: 'success',
      warning: 'warning',
      error: 'error',
      confirmation: 'confirmation',
      info: 'info',
    };

    return icons[type];
  }
}
