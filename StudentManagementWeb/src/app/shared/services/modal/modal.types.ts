export type ModalType = 'info' | 'success' | 'warning' | 'error' | 'confirmation';
export type ModalSize = 'small' | 'medium' | 'large';

export interface ModalOptions {
  readonly type: ModalType;
  readonly title: string;
  readonly message: string;
  readonly confirmText?: string;
  readonly cancelText?: string;
  readonly destructive?: boolean;
  readonly dismissible?: boolean;
  readonly size?: ModalSize;
}

export interface ModalState extends ModalOptions {
  readonly confirmation: boolean;
}
