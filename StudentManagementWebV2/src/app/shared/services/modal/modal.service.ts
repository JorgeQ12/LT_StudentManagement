import { DOCUMENT } from '@angular/common';
import { inject, Injectable, signal } from '@angular/core';
import { ModalOptions, ModalState, ModalType } from './modal.types';

@Injectable({ providedIn: 'root' })
export class ModalService {
  private readonly document = inject(DOCUMENT);
  private readonly current = signal<ModalState | null>(null);
  private resolver: ((result: boolean) => void) | null = null;
  private previousFocus: HTMLElement | null = null;

  readonly state = this.current.asReadonly();

  info(options: Omit<ModalOptions, 'type'>): Promise<void> {
    return this.message('info', options);
  }

  success(options: Omit<ModalOptions, 'type'>): Promise<void> {
    return this.message('success', options);
  }

  warning(options: Omit<ModalOptions, 'type'>): Promise<void> {
    return this.message('warning', options);
  }

  error(options: Omit<ModalOptions, 'type'>): Promise<void> {
    return this.message('error', options);
  }

  confirm(options: Omit<ModalOptions, 'type'>): Promise<boolean> {
    return this.open({ ...options, type: 'confirmation', confirmation: true });
  }

  resolve(result: boolean): void {
    const resolver = this.resolver;
    this.resolver = null;
    this.current.set(null);
    resolver?.(result);
    queueMicrotask(() => this.previousFocus?.focus());
  }

  private async message(
    type: Exclude<ModalType, 'confirmation'>,
    options: Omit<ModalOptions, 'type'>,
  ): Promise<void> {
    await this.open({ ...options, type, confirmation: false });
  }

  private open(state: ModalState): Promise<boolean> {
    if (this.resolver) {
      this.resolver(false);
    }
    this.previousFocus =
      this.document.activeElement instanceof HTMLElement ? this.document.activeElement : null;
    this.current.set(state);
    return new Promise<boolean>((resolve) => {
      this.resolver = resolve;
    });
  }
}
