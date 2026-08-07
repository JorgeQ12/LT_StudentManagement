import { Component } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { DatePickerComponent } from './date-picker.component';

@Component({
  imports: [ReactiveFormsModule, DatePickerComponent],
  template: ` <app-date-picker id="birth-date" [formControl]="control" max="2020-05-31" /> `,
})
class DatePickerHostComponent {
  readonly control = new FormControl('2020-05-10');
}

describe('DatePickerComponent', () => {
  let fixture: ComponentFixture<DatePickerHostComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DatePickerHostComponent],
    }).compileComponents();
    fixture = TestBed.createComponent(DatePickerHostComponent);
    fixture.detectChanges();
  });

  it('renders the form value without a native date input', () => {
    const trigger = fixture.nativeElement.querySelector('.date-picker__trigger') as HTMLElement;

    expect(trigger.textContent).toContain('10 de mayo de 2020');
    expect(fixture.nativeElement.querySelector('input[type="date"]')).toBeNull();
  });

  it('allows selecting a day from the custom calendar', () => {
    const trigger = fixture.nativeElement.querySelector('.date-picker__trigger') as HTMLElement;
    trigger.click();
    fixture.detectChanges();

    const day = Array.from(
      fixture.nativeElement.querySelectorAll('.date-picker__days button'),
    ).find((element) => (element as HTMLElement).textContent?.trim() === '15') as HTMLElement;
    day.click();
    fixture.detectChanges();

    expect(fixture.componentInstance.control.value).toBe('2020-05-15');
  });

  it('provides custom month and year navigation views', () => {
    const trigger = fixture.nativeElement.querySelector('.date-picker__trigger') as HTMLElement;
    trigger.click();
    fixture.detectChanges();

    const period = fixture.nativeElement.querySelector('.date-picker__period') as HTMLElement;
    period.click();
    fixture.detectChanges();
    expect(fixture.nativeElement.textContent).toContain('Ene');

    period.click();
    fixture.detectChanges();
    expect(fixture.nativeElement.textContent).toContain('2020');
  });
});
