import { AbstractControl } from '@angular/forms';

export function getControlError(control: AbstractControl, label: string): string | null {
  if (!control.touched || !control.errors) {
    return null;
  }
  if (control.hasError('required')) return `${label} es obligatorio.`;
  if (control.hasError('email')) return 'Ingresa un correo electrónico válido.';
  if (control.hasError('minlength')) return `${label} no cumple la longitud mínima.`;
  if (control.hasError('maxlength')) return `${label} supera la longitud permitida.`;
  if (control.hasError('pattern')) return `${label} tiene un formato inválido.`;
  if (control.hasError('pastDate')) return 'La fecha debe ser anterior a hoy.';
  if (control.hasError('passwordStrength'))
    return 'Usa al menos 10 caracteres, una mayúscula, una minúscula y un número.';
  return `${label} es inválido.`;
}

export function pastDateValidator(control: AbstractControl): { pastDate: true } | null {
  if (!control.value) return null;
  return String(control.value) < new Date().toISOString().slice(0, 10) ? null : { pastDate: true };
}

export function passwordStrengthValidator(
  control: AbstractControl,
): { passwordStrength: true } | null {
  const value = String(control.value ?? '');
  return value.length >= 10 && /[A-Z]/.test(value) && /[a-z]/.test(value) && /\d/.test(value)
    ? null
    : { passwordStrength: true };
}
