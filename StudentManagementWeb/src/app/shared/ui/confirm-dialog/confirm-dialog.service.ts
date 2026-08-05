import { Dialog } from '@angular/cdk/dialog';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { ConfirmDialog, ConfirmDialogData } from './confirm-dialog';

@Injectable({ providedIn: 'root' })
export class ConfirmDialogService {
  private readonly dialog = inject(Dialog);

  open(data: ConfirmDialogData): Observable<boolean | undefined> {
    return this.dialog.open<boolean>(ConfirmDialog, {
      data,
      disableClose: true,
      ariaLabel: data.title,
    }).closed;
  }
}
