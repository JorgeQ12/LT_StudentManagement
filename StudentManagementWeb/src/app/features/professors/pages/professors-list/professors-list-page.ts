import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Icon } from '@shared/ui/icon';
import { firstValueFrom, switchMap, tap } from 'rxjs';

import { CatalogStatus, Professor } from '@core/api/api.models';
import { problemFromError } from '@core/api/api-error';
import { DataTable, DataTableColumn } from '@shared/ui/data-table';
import { ConfirmDialogService } from '@shared/ui/confirm-dialog';
import { EmptyState } from '@shared/ui/empty-state';
import { LoadingState } from '@shared/ui/loading-state';
import { PageHeader } from '@shared/ui/page-header';
import { ToastService } from '@shared/ui/toast';

import { ProfessorsFacade } from '../../facades/professors.facade';

@Component({
  selector: 'sm-professors-list-page',
  imports: [DataTable, EmptyState, LoadingState, Icon, PageHeader, RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './professors-list-page.html',
  styleUrl: './professors-list-page.css',
})
export class ProfessorsListPage {
  private readonly facade = inject(ProfessorsFacade);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly confirmDialog = inject(ConfirmDialogService);
  private readonly toast = inject(ToastService);

  protected readonly copy = {
    eyebrow: 'Equipo docente',
    title: 'Profesores',
    description: 'Administra profesores y consulta su carga académica.',
    create: 'Crear profesor',
  } as const;

  protected readonly columns: DataTableColumn<Professor>[] = [
    {
      header: 'Profesor',
      render: (p) => `${p.firstName} ${p.lastName}`,
      kind: 'strong',
    },
    { header: 'Materias asignadas', render: (p) => String(p.assignedCourseCount), kind: 'muted' },
    { header: 'Disponibilidad', render: () => 'Máximo 2 por programa', kind: 'muted' },
  ];

  protected readonly items = this.facade.items;
  protected readonly pageNumber = signal(1);
  protected readonly totalCount = this.facade.totalCount;
  protected readonly search = signal('');
  protected readonly status = signal<CatalogStatus | ''>('');
  protected readonly loading = this.facade.loading;
  protected readonly error = this.facade.error;
  protected readonly busyId = this.facade.busyId;
  protected readonly pageSize = 10;

  constructor() {
    this.route.queryParamMap
      .pipe(
        tap((params) => {
          this.pageNumber.set(Math.max(1, Number(params.get('page') || 1)));
          this.search.set(params.get('search') || '');
          this.status.set((params.get('status') as CatalogStatus | null) || '');
        }),
        switchMap(() =>
          this.facade.load$({
            pageNumber: this.pageNumber(),
            pageSize: this.pageSize,
            search: this.search(),
            status: this.status(),
          }),
        ),
        takeUntilDestroyed(),
      )
      .subscribe();
  }

  protected async load(): Promise<void> {
    await this.facade.load({
      pageNumber: this.pageNumber(),
      pageSize: this.pageSize,
      search: this.search(),
      status: this.status(),
    });
  }

  protected applyFilters(event: Event, search: string, status: string): void {
    event.preventDefault();
    void this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {
        page: 1,
        search: search || null,
        status: status || null,
      },
    });
  }

  protected goToPage(page: number): void {
    void this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { page },
      queryParamsHandling: 'merge',
    });
  }

  protected async toggleStatus(item: Professor): Promise<void> {
    const activating = item.status === 'Inactive';
    if (!activating) {
      const confirmed = await firstValueFrom(
        this.confirmDialog.open({
          title: `¿Desactivar este registro?`,
          message:
            'Se conservará el historial. La operación puede ser rechazada si existen relaciones académicas activas.',
          confirmLabel: 'Sí, desactivar',
          tone: 'danger',
        }),
      );
      if (!confirmed) {
        return;
      }
    }

    try {
      await this.facade.changeStatus(item, activating);
      this.toast.success(activating ? 'Registro activado' : 'Registro desactivado');
      await this.load();
    } catch (error) {
      const problem = problemFromError(error);
      this.toast.error(problem.title, problem.detail);
    }
  }
}
