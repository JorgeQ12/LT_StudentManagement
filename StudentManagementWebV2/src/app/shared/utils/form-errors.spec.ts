import { FormControl } from '@angular/forms';
import { passwordStrengthValidator, pastDateValidator } from './form-errors';

describe('shared form validators', () => {
  it('accepts only dates before today', () => {
    expect(pastDateValidator(new FormControl('2000-01-01'))).toBeNull();
    expect(pastDateValidator(new FormControl('2999-01-01'))).toEqual({ pastDate: true });
  });

  it('requires a sufficiently strong password', () => {
    expect(passwordStrengthValidator(new FormControl('ClaveSegura1'))).toBeNull();
    expect(passwordStrengthValidator(new FormControl('debil'))).toEqual({
      passwordStrength: true,
    });
  });
});
