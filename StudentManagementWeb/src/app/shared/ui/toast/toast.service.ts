import { computed, Injectable, signal } from '@angular/core';

export type ToastTone = 'success' | 'error' | 'info';

export interface ToastMessage {
  readonly id: number;
  readonly tone: ToastTone;
  readonly title: string;
  readonly message?: string;
}

@Injectable({ providedIn: 'root' })
export class ToastService {
  private readonly messagesState = signal<readonly ToastMessage[]>([]);
  private sequence = 0;

  readonly messages = computed(() => this.messagesState());

  success(title: string, message?: string): void {
    this.show('success', title, message);
  }

  error(title: string, message?: string): void {
    this.show('error', title, message, 7000);
  }

  info(title: string, message?: string): void {
    this.show('info', title, message);
  }

  dismiss(id: number): void {
    this.messagesState.update((messages) => messages.filter((message) => message.id !== id));
  }

  private show(tone: ToastTone, title: string, message?: string, duration = 4500): void {
    const id = ++this.sequence;
    this.messagesState.update((messages) => [...messages, { id, tone, title, message }]);
    window.setTimeout(() => this.dismiss(id), duration);
  }
}
