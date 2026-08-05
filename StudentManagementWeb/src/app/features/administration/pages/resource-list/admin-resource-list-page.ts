import {
  ChangeDetectionStrategy,
  Component,
  computed,
  DestroyRef,
  inject,
  signal,
} from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { NgIcon } from '@ng-icons/core';
import { firstValueFrom, switchMap, tap } from 'rxjs';

import { AccountStatus, CatalogStatus } from '@core/api/api.models';
import { problemFromError } from '@core/api/api-error';
import { ConfirmDialogService } from '@shared/ui/confirm-dialog';
import { EmptyState } from '@shared/ui/empty-state';
import { LoadingState } from '@shared/ui/loading-state';
import { PageHeader } from '@shared/ui/page-header';
import { ToastService } from '@shared/ui/toast';

import { AdminResourceTable } from '../../components/resource-table/admin-resource-table';
import {
  AdminEntity,
  AdminResource,
  AdministrationFacade,
} from '../../facades/administration.facade';

const RESOURCE_COPY: Record<
  AdminResource,
  {
    readonly eyebrow: string;
    readonly title: string;
    readonly description: string;
    readonly create: string;
  }
> = {
  students: {
    eyebrow: 'Comunidad estudiantil',
    title: 'Estudiantes',
    description: 'Consulta perfiles y administra el estado de sus cuentas.',
    create: 'Crear estudiante',
  },
  'academic-programs': {
    eyebrow: 'Oferta académica',
    title: 'Programas académicos',
    description: 'Mantén organizados los programas disponibles para inscripción.',
    create: 'Crear programa',
  },
  courses: {
    eyebrow: 'Catálogo académico',
    title: 'Materias',
    description: 'Gestiona materias, créditos y asignaciones docentes.',
    create: 'Crear materia',
  },
  professors: {
    eyebrow: 'Equipo docente',
    title: 'Profesores',
    description: 'Administra profesores y consulta su carga académica.',
    create: 'Crear profesor',
  },
};

@Component({
  selector: 'sm-admin-resource-list-page',
  imports: [AdminResourceTable, EmptyState, LoadingState, NgIcon, PageHeader, RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './admin-resource-list-page.html',
  styleUrl: './admin-resource-list-page.css',
})
export class AdminResourceListPage {
  private readonly facade = inject(AdministrationFacade);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly destroyRef = inject(DestroyRef);
  private readonly confirmDialog = inject(ConfirmDialogService);
  private readonly toast = inject(ToastService);

  protected readonly resource = signal<AdminResource>(
    this.route.snapshot.data['resource'] as AdminResource,
  );
  protected readonly copy = computed(() => RESOURCE_COPY[this.resource()]);
  protected readonly items = this.facade.items;
  protected readonly pageNumber = signal(1);
  protected readonly totalCount = this.facade.totalCount;
  protected readonly search = signal('');
  protected readonly status = signal<AccountStatus | CatalogStatus | ''>('');
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
          this.status.set((params.get('status') as AccountStatus | CatalogStatus | null) || '');
        }),
        switchMap(() =>
          this.facade.load$(this.resource(), {
            pageNumber: this.pageNumber(),
            pageSize: this.pageSize,
            search: this.search(),
            status: this.status(),
          }),
        ),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe();
  }

  protected async load(): Promise<void> {
    await this.facade.load(this.resource(), {
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

  protected async toggleStatus(item: AdminEntity): Promise<void> {
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
      await this.facade.changeStatus(this.resource(), item, activating);
      this.toast.success(activating ? 'Registro activado' : 'Registro desactivado');
      await this.load();
    } catch (error) {
      const problem = problemFromError(error);
      this.toast.error(problem.title, problem.detail);
    }
  }
}
