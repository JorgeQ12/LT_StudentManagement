import { ChangeDetectionStrategy, Component, computed, input } from '@angular/core';
import { NgIcon } from '@ng-icons/core';
import {
  lucideArrowRight,
  lucideBookOpen,
  lucideBookOpenCheck,
  lucideCalendarDays,
  lucideCheck,
  lucideChevronDown,
  lucideChevronLeft,
  lucideChevronRight,
  lucideCircleCheck,
  lucideCircleQuestionMark,
  lucideCircleX,
  lucideContactRound,
  lucideFileQuestion,
  lucideGraduationCap,
  lucideInfo,
  lucideLayoutDashboard,
  lucideLogIn,
  lucideLogOut,
  lucideMenu,
  lucidePanelsTopLeft,
  lucidePencil,
  lucidePlus,
  lucidePower,
  lucideSave,
  lucideSchool,
  lucideSearch,
  lucideTriangleAlert,
  lucideUserRound,
  lucideUserPlus,
  lucideUsers,
  lucideX,
  lucideFilter,
} from '@ng-icons/lucide';

const APP_ICONS = {
  arrowRight: lucideArrowRight,
  calendar: lucideCalendarDays,
  check: lucideCheck,
  chevronDown: lucideChevronDown,
  chevronLeft: lucideChevronLeft,
  chevronRight: lucideChevronRight,
  courses: lucideBookOpen,
  enrollment: lucideBookOpenCheck,
  success: lucideCircleCheck,
  confirmation: lucideCircleQuestionMark,
  error: lucideCircleX,
  professors: lucideContactRound,
  empty: lucideFileQuestion,
  graduation: lucideGraduationCap,
  info: lucideInfo,
  dashboard: lucideLayoutDashboard,
  logout: lucideLogOut,
  menu: lucideMenu,
  programs: lucidePanelsTopLeft,
  activate: lucidePower,
  add: lucidePlus,
  cancel: lucideX,
  edit: lucidePencil,
  filter: lucideFilter,
  login: lucideLogIn,
  save: lucideSave,
  school: lucideSchool,
  register: lucideUserPlus,
  search: lucideSearch,
  warning: lucideTriangleAlert,
  profile: lucideUserRound,
  students: lucideUsers,
} as const;

export type AppIconName = keyof typeof APP_ICONS;

@Component({
  selector: 'app-icon',
  imports: [NgIcon],
  template: `
    <ng-icon
      [svg]="svg()"
      [size]="size()"
      [strokeWidth]="strokeWidth()"
      [attr.aria-hidden]="decorative() ? 'true' : null"
      [attr.aria-label]="decorative() ? null : label()"
      [attr.role]="decorative() ? null : 'img'"
    />
  `,
  styles: `
    :host {
      display: inline-grid;
      place-items: center;
      line-height: 0;
    }
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class IconComponent {
  readonly name = input.required<AppIconName>();
  readonly size = input('1.25rem');
  readonly strokeWidth = input(1.8);
  readonly decorative = input(true);
  readonly label = input<string | null>(null);
  protected readonly svg = computed(() => APP_ICONS[this.name()]);
}
