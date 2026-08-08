import { Component } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CustomSelectComponent, CustomSelectOption } from './custom-select.component';

@Component({
  imports: [ReactiveFormsModule, CustomSelectComponent],
  template: ` <app-custom-select id="status" [formControl]="control" [options]="options" /> `,
})
class SelectHostComponent {
  readonly control = new FormControl('');
  readonly options: readonly CustomSelectOption[] = [
    { value: '', label: 'Todos' },
    { value: 'Active', label: 'Activos' },
    { value: 'Inactive', label: 'Inactivos' },
  ];
}

describe('CustomSelectComponent', () => {
  let fixture: ComponentFixture<SelectHostComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({ imports: [SelectHostComponent] }).compileComponents();
    fixture = TestBed.createComponent(SelectHostComponent);
    fixture.detectChanges();
  });

  it('writes a selected option back to the reactive form', () => {
    const trigger = fixture.nativeElement.querySelector('.custom-select__trigger') as HTMLElement;
    trigger.click();
    fixture.detectChanges();

    const options = fixture.nativeElement.querySelectorAll('.custom-select__option');
    (options[1] as HTMLElement).click();
    fixture.detectChanges();

    expect(fixture.componentInstance.control.value).toBe('Active');
    expect(trigger.textContent).toContain('Activos');
  });

  it('supports arrow and enter keyboard selection', () => {
    const trigger = fixture.nativeElement.querySelector('.custom-select__trigger') as HTMLElement;
    trigger.dispatchEvent(new KeyboardEvent('keydown', { key: 'ArrowDown', bubbles: true }));
    trigger.dispatchEvent(new KeyboardEvent('keydown', { key: 'ArrowDown', bubbles: true }));
    trigger.dispatchEvent(new KeyboardEvent('keydown', { key: 'Enter', bubbles: true }));
    fixture.detectChanges();

    expect(fixture.componentInstance.control.value).toBe('Active');
  });
});
