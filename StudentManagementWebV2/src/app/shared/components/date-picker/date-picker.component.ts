import {
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  HostListener,
  computed,
  forwardRef,
  inject,
  input,
  signal,
} from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { IconComponent } from '../icon/icon.component';

type CalendarView = 'days' | 'months' | 'years';

interface CalendarDay {
  readonly iso: string;
  readonly number: number;
  readonly outside: boolean;
  readonly disabled: boolean;
  readonly today: boolean;
  readonly selected: boolean;
  readonly label: string;
}

const MONTHS = [
  'Enero',
  'Febrero',
  'Marzo',
  'Abril',
  'Mayo',
  'Junio',
  'Julio',
  'Agosto',
  'Septiembre',
  'Octubre',
  'Noviembre',
  'Diciembre',
] as const;

const WEEKDAYS = ['L', 'M', 'X', 'J', 'V', 'S', 'D'] as const;

@Component({
  selector: 'app-date-picker',
  imports: [IconComponent],
  templateUrl: './date-picker.component.html',
  styleUrl: './date-picker.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => DatePickerComponent),
      multi: true,
    },
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DatePickerComponent implements ControlValueAccessor {
  private readonly element = inject(ElementRef<HTMLElement>);
  readonly id = input.required<string>();
  readonly placeholder = input('Selecciona una fecha');
  readonly min = input<string | null>(null);
  readonly max = input<string | null>(null);
  readonly ariaLabel = input<string | null>(null);

  protected readonly weekDays = WEEKDAYS;
  protected readonly months = MONTHS;
  protected readonly value = signal('');
  protected readonly open = signal(false);
  protected readonly disabled = signal(false);
  protected readonly view = signal<CalendarView>('days');
  protected readonly viewYear = signal(new Date().getFullYear());
  protected readonly viewMonth = signal(new Date().getMonth());
  protected readonly displayValue = computed(() => this.formatDisplayDate(this.value()));
  protected readonly monthLabel = computed(
    () => `${MONTHS[this.viewMonth()]} de ${this.viewYear()}`,
  );
  protected readonly yearRangeStart = computed(() => Math.floor(this.viewYear() / 12) * 12);
  protected readonly yearRange = computed(() =>
    Array.from({ length: 12 }, (_, index) => this.yearRangeStart() + index),
  );
  protected readonly calendarDays = computed<readonly CalendarDay[]>(() => {
    const year = this.viewYear();
    const month = this.viewMonth();
    const firstDay = new Date(year, month, 1);
    const mondayOffset = (firstDay.getDay() + 6) % 7;
    const start = new Date(year, month, 1 - mondayOffset);
    const today = this.toIsoDate(new Date());

    return Array.from({ length: 42 }, (_, index) => {
      const date = new Date(start.getFullYear(), start.getMonth(), start.getDate() + index);
      const iso = this.toIsoDate(date);
      return {
        iso,
        number: date.getDate(),
        outside: date.getMonth() !== month,
        disabled: !this.isAllowed(iso),
        today: iso === today,
        selected: iso === this.value(),
        label: new Intl.DateTimeFormat('es-CO', {
          weekday: 'long',
          day: 'numeric',
          month: 'long',
          year: 'numeric',
        }).format(date),
      };
    });
  });

  private onChange: (value: string) => void = () => undefined;
  private onTouched: () => void = () => undefined;

  writeValue(value: string | null): void {
    this.value.set(value ?? '');
    if (value) this.setViewFromIso(value);
  }

  registerOnChange(callback: (value: string) => void): void {
    this.onChange = callback;
  }

  registerOnTouched(callback: () => void): void {
    this.onTouched = callback;
  }

  setDisabledState(disabled: boolean): void {
    this.disabled.set(disabled);
    if (disabled) this.open.set(false);
  }

  protected toggle(): void {
    if (this.disabled()) return;
    if (this.open()) {
      this.close();
      return;
    }
    const initial = this.value() || this.max() || this.toIsoDate(new Date());
    this.setViewFromIso(initial);
    this.view.set('days');
    this.open.set(true);
  }

  protected chooseDay(day: CalendarDay): void {
    if (day.disabled) return;
    this.value.set(day.iso);
    this.onChange(day.iso);
    this.onTouched();
    this.close();
  }

  protected chooseMonth(month: number): void {
    if (this.monthDisabled(month)) return;
    this.viewMonth.set(month);
    this.view.set('days');
  }

  protected chooseYear(year: number): void {
    if (this.yearDisabled(year)) return;
    this.viewYear.set(year);
    this.view.set('months');
  }

  protected changeHeaderView(): void {
    if (this.view() === 'days') this.view.set('months');
    else if (this.view() === 'months') this.view.set('years');
  }

  protected navigate(direction: -1 | 1): void {
    if (!this.canNavigate(direction)) return;
    if (this.view() === 'days') {
      const target = new Date(this.viewYear(), this.viewMonth() + direction, 1);
      this.viewYear.set(target.getFullYear());
      this.viewMonth.set(target.getMonth());
    } else if (this.view() === 'months') {
      this.viewYear.update((year) => year + direction);
    } else {
      this.viewYear.update((year) => year + direction * 12);
    }
  }

  protected canNavigate(direction: -1 | 1): boolean {
    if (this.view() === 'days') {
      const target = new Date(this.viewYear(), this.viewMonth() + direction, 1);
      return this.monthHasAllowedDate(target.getFullYear(), target.getMonth());
    }
    if (this.view() === 'months') return !this.yearDisabled(this.viewYear() + direction);
    const targetStart = this.yearRangeStart() + direction * 12;
    return Array.from({ length: 12 }, (_, index) => targetStart + index).some(
      (year) => !this.yearDisabled(year),
    );
  }

  protected monthDisabled(month: number): boolean {
    return !this.monthHasAllowedDate(this.viewYear(), month);
  }

  protected yearDisabled(year: number): boolean {
    const first = `${year}-01-01`;
    const last = `${year}-12-31`;
    return Boolean((this.min() && last < this.min()!) || (this.max() && first > this.max()!));
  }

  protected handleKeydown(event: KeyboardEvent): void {
    if (event.key === 'Escape' && this.open()) {
      event.preventDefault();
      this.close();
    }
  }

  protected markTouched(): void {
    this.onTouched();
  }

  @HostListener('document:pointerdown', ['$event'])
  protected closeOnOutsideClick(event: PointerEvent): void {
    if (!this.element.nativeElement.contains(event.target as Node)) this.close();
  }

  private close(): void {
    this.open.set(false);
  }

  private setViewFromIso(iso: string): void {
    const date = this.parseIsoDate(iso);
    if (!date) return;
    this.viewYear.set(date.getFullYear());
    this.viewMonth.set(date.getMonth());
  }

  private formatDisplayDate(iso: string): string {
    const date = this.parseIsoDate(iso);
    if (!date) return '';
    return new Intl.DateTimeFormat('es-CO', {
      day: '2-digit',
      month: 'long',
      year: 'numeric',
    }).format(date);
  }

  private parseIsoDate(iso: string): Date | null {
    const match = /^(\d{4})-(\d{2})-(\d{2})$/.exec(iso);
    if (!match) return null;
    const date = new Date(Number(match[1]), Number(match[2]) - 1, Number(match[3]));
    return Number.isNaN(date.getTime()) ? null : date;
  }

  private toIsoDate(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  private isAllowed(iso: string): boolean {
    return !(this.min() && iso < this.min()!) && !(this.max() && iso > this.max()!);
  }

  private monthHasAllowedDate(year: number, month: number): boolean {
    const first = this.toIsoDate(new Date(year, month, 1));
    const last = this.toIsoDate(new Date(year, month + 1, 0));
    return !(this.min() && last < this.min()!) && !(this.max() && first > this.max()!);
  }
}
