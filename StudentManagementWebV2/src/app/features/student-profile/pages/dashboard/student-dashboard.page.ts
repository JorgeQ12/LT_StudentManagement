import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink } from '@angular/router';
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
}
