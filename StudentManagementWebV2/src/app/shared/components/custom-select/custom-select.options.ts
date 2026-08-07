import { CustomSelectOption } from './custom-select.component';

export const STATUS_FILTER_OPTIONS: readonly CustomSelectOption[] = [
  { value: '', label: 'Todos' },
  { value: 'Active', label: 'Activos' },
  { value: 'Inactive', label: 'Inactivos' },
];
