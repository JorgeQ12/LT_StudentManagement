import { ChangeDetectionStrategy, Component, computed, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Icon } from '@shared/ui/icon';

@Component({
  selector: 'sm-error-page',
  imports: [Icon, RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './error-page.html',
  styleUrl: './error-page.css',
})
export class ErrorPage {
  private readonly route = inject(ActivatedRoute);
  protected readonly status = computed(() => String(this.route.snapshot.data['status'] ?? '404'));
  protected readonly title = computed(() => String(this.route.snapshot.data['title'] ?? 'Error'));
  protected readonly description = computed(() =>
    String(this.route.snapshot.data['description'] ?? ''),
  );
}
