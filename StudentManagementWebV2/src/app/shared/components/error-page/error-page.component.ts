import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ButtonComponent } from '../button/button.component';

@Component({
  selector: 'app-error-page',
  imports: [RouterLink, ButtonComponent],
  templateUrl: './error-page.component.html',
  styleUrl: './error-page.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ErrorPageComponent {
  private readonly data = inject(ActivatedRoute).snapshot.data;
  protected readonly code = String(this.data['code'] ?? '404');
  protected readonly title = String(this.data['title'] ?? 'Página no encontrada');
  protected readonly description = String(
    this.data['description'] ?? 'La dirección solicitada no existe o cambió.',
  );
}
