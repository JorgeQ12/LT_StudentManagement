import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthFacade } from '../../../../core/auth/auth.facade';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { IconComponent } from '../../../../shared/components/icon/icon.component';
import { PageHeaderComponent } from '../../../../shared/components/page-header/page-header.component';
@Component({
  selector: 'app-student-dashboard-page',
  imports: [RouterLink, ButtonComponent, PageHeaderComponent, IconComponent],
  templateUrl: './student-dashboard.page.html',
  styleUrl: './student-dashboard.page.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class StudentDashboardPage {
  protected readonly user = inject(AuthFacade).user;
}
