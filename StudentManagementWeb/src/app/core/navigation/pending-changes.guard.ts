import { inject } from '@angular/core';
import { CanDeactivateFn } from '@angular/router';
import { firstValueFrom } from 'rxjs';

import { ConfirmDialogService } from '@shared/ui/confirm-dialog';

export interface PendingChangesAware {
  hasPendingChanges(): boolean;
}

export const pendingChangesGuard: CanDeactivateFn<PendingChangesAware> = (component) => {
  if (!component.hasPendingChanges()) {
    return true;
  }

  return firstValueFrom(
    inject(ConfirmDialogService).open({
      title: '¿Salir sin guardar?',
      message: 'Los cambios que hiciste en este formulario se perderán.',
      confirmLabel: 'Sí, salir',
      tone: 'danger',
    }),
  ).then(Boolean);
};
