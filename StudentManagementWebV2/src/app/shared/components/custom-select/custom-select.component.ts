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

export interface CustomSelectOption {
  readonly value: string;
  readonly label: string;
  readonly description?: string;
  readonly disabled?: boolean;
}

@Component({
  selector: 'app-custom-select',
  imports: [IconComponent],
  templateUrl: './custom-select.component.html',
  styleUrl: './custom-select.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CustomSelectComponent),
      multi: true,
    },
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomSelectComponent implements ControlValueAccessor {
  private readonly element = inject(ElementRef<HTMLElement>);
  readonly id = input.required<string>();
  readonly options = input.required<readonly CustomSelectOption[]>();
  readonly placeholder = input('Selecciona una opción');
  readonly ariaLabel = input<string | null>(null);

  protected readonly value = signal('');
  protected readonly open = signal(false);
  protected readonly disabled = signal(false);
  protected readonly focusedIndex = signal(-1);
  protected readonly selectedOption = computed(() =>
    this.options().find((option) => option.value === this.value()),
  );
  protected readonly listId = computed(() => `${this.id()}-options`);
  protected readonly activeDescendant = computed(() => {
    const index = this.focusedIndex();
    return this.open() && index >= 0 ? `${this.id()}-option-${index}` : null;
  });

  private onChange: (value: string) => void = () => undefined;
  private onTouched: () => void = () => undefined;

  writeValue(value: string | null): void {
    this.value.set(value ?? '');
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
    if (this.open()) this.close();
    else this.show();
  }

  protected select(option: CustomSelectOption): void {
    if (option.disabled) return;
    this.value.set(option.value);
    this.onChange(option.value);
    this.onTouched();
    this.close();
  }

  protected handleKeydown(event: KeyboardEvent): void {
    if (this.disabled()) return;

    switch (event.key) {
      case 'ArrowDown':
        event.preventDefault();
        if (!this.open()) this.show();
        else this.moveFocus(1);
        break;
      case 'ArrowUp':
        event.preventDefault();
        if (!this.open()) this.show();
        else this.moveFocus(-1);
        break;
      case 'Home':
        if (this.open()) {
          event.preventDefault();
          this.focusBoundary(1);
        }
        break;
      case 'End':
        if (this.open()) {
          event.preventDefault();
          this.focusBoundary(-1);
        }
        break;
      case 'Enter':
      case ' ':
        event.preventDefault();
        if (!this.open()) this.show();
        else {
          const option = this.options()[this.focusedIndex()];
          if (option) this.select(option);
        }
        break;
      case 'Escape':
        if (this.open()) {
          event.preventDefault();
          this.close();
        }
        break;
    }
  }

  protected markTouched(): void {
    this.onTouched();
  }

  @HostListener('document:pointerdown', ['$event'])
  protected closeOnOutsideClick(event: PointerEvent): void {
    if (!this.element.nativeElement.contains(event.target as Node)) this.close();
  }

  private show(): void {
    this.open.set(true);
    const selectedIndex = this.options().findIndex(
      (option) => option.value === this.value() && !option.disabled,
    );
    this.focusedIndex.set(selectedIndex >= 0 ? selectedIndex : this.firstEnabledIndex());
  }

  private close(): void {
    this.open.set(false);
    this.focusedIndex.set(-1);
  }

  private moveFocus(direction: 1 | -1): void {
    const options = this.options();
    if (!options.length) return;
    let index = this.focusedIndex();
    let attempts = 0;
    while (attempts < options.length) {
      index = (index + direction + options.length) % options.length;
      if (!options[index].disabled) {
        this.focusedIndex.set(index);
        return;
      }
      attempts += 1;
    }
  }

  private focusBoundary(direction: 1 | -1): void {
    const options = this.options();
    const start = direction === 1 ? 0 : options.length - 1;
    const end = direction === 1 ? options.length : -1;
    for (let index = start; index !== end; index += direction) {
      if (!options[index].disabled) {
        this.focusedIndex.set(index);
        return;
      }
    }
  }

  private firstEnabledIndex(): number {
    return this.options().findIndex((option) => !option.disabled);
  }
}
