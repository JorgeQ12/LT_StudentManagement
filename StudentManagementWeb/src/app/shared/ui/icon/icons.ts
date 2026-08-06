export type IconName =
  | 'arrow-left'
  | 'book'
  | 'book-check'
  | 'book-marked'
  | 'book-open'
  | 'calendar-days'
  | 'check'
  | 'chevron-left'
  | 'chevron-right'
  | 'circle-alert'
  | 'circle-check'
  | 'clock'
  | 'close'
  | 'eye'
  | 'graduation-cap'
  | 'id-card'
  | 'inbox'
  | 'info'
  | 'layout-dashboard'
  | 'library'
  | 'loader-circle'
  | 'log-out'
  | 'mail'
  | 'menu'
  | 'pencil'
  | 'phone'
  | 'plus'
  | 'power'
  | 'power-off'
  | 'presentation'
  | 'refresh-cw'
  | 'save'
  | 'school'
  | 'search'
  | 'shield-check'
  | 'triangle-alert'
  | 'user-cog'
  | 'user-plus'
  | 'user-round'
  | 'users';

/**
 * Stroke-based, hand-authored icons drawn on a shared 24x24 viewBox. They use
 * `currentColor` so they inherit the surrounding text `color`.
 */
export const ICONS: Record<IconName, string> = {
  'arrow-left': '<line x1="20" y1="12" x2="5" y2="12"/><polyline points="12 5 5 12 12 19"/>',
  book: '<path d="M4 19.5A2.5 2.5 0 0 1 6.5 17H20"/><path d="M6.5 2H20v20H6.5A2.5 2.5 0 0 1 4 19.5v-15A2.5 2.5 0 0 1 6.5 2Z"/>',
  'book-check':
    '<path d="M20 13V6a2 2 0 0 0-2-2H6a2 2 0 0 0-2 2v14a2 2 0 0 1 2-2h14"/><path d="m9 11 2 2 4-4"/>',
  'book-marked':
    '<path d="M4 19.5A2.5 2.5 0 0 1 6.5 17H20"/><path d="M6.5 2H20v20H6.5A2.5 2.5 0 0 1 4 19.5v-15A2.5 2.5 0 0 1 6.5 2Z"/><path d="M9 4h2v4l-1.5-1-1.5 1Z"/>',
  'book-open':
    '<path d="M2 6a2 2 0 0 1 2-2h7a1 1 0 0 1 1 1v14a1 1 0 0 0-1-1H4a2 2 0 0 1-2-2Z"/><path d="M22 6a2 2 0 0 0-2-2h-7a1 1 0 0 0-1 1v14a1 1 0 0 1 1-1h7a2 2 0 0 0 2-2Z"/>',
  'calendar-days':
    '<rect x="3" y="5" width="18" height="16" rx="2"/><path d="M16 3v4"/><path d="M8 3v4"/><path d="M3 11h18"/><path d="M8 15h.01"/><path d="M12 15h.01"/><path d="M16 15h.01"/>',
  check: '<path d="m5 12 5 5L20 7"/>',
  'chevron-left': '<path d="m15 18-6-6 6-6"/>',
  'chevron-right': '<path d="m9 18 6-6-6-6"/>',
  'circle-alert': '<circle cx="12" cy="12" r="9"/><path d="M12 8v4"/><path d="M12 16h.01"/>',
  'circle-check': '<circle cx="12" cy="12" r="9"/><path d="m8.5 12 2.5 2.5 4.5-5"/>',
  clock: '<circle cx="12" cy="12" r="9"/><path d="M12 7v5l3 2"/>',
  close: '<path d="M18 6 6 18"/><path d="m6 6 12 12"/>',
  eye: '<path d="M2 12s3.5-6 10-6 10 6 10 6-3.5 6-10 6-10-6-10-6Z"/><circle cx="12" cy="12" r="3"/>',
  'graduation-cap':
    '<path d="M22 10 12 5 2 10l10 5Z"/><path d="M6 12v5c0 1.7 2.7 3 6 3s6-1.3 6-3v-5"/><path d="M22 10v6"/>',
  'id-card':
    '<rect x="2" y="4" width="20" height="16" rx="2"/><circle cx="8" cy="10" r="2"/><path d="M5 16a3 3 0 0 1 3-3h0a3 3 0 0 1 3 3"/><path d="M14 9h5"/><path d="M14 13h5"/>',
  inbox:
    '<path d="M22 12h-6l-2 3h-4l-2-3H2"/><path d="M5.5 5h13L22 12v6a2 2 0 0 1-2 2H4a2 2 0 0 1-2-2v-6Z"/>',
  info: '<circle cx="12" cy="12" r="9"/><path d="M12 8h.01"/><path d="M11 12h1v4h1"/>',
  'layout-dashboard':
    '<rect x="3" y="3" width="7" height="9" rx="1"/><rect x="14" y="3" width="7" height="5" rx="1"/><rect x="14" y="12" width="7" height="9" rx="1"/><rect x="3" y="16" width="7" height="5" rx="1"/>',
  library: '<path d="m16 6 4 14"/><path d="M12 6v14"/><path d="M8 8v12"/><path d="M4 4v16"/>',
  'loader-circle': '<path d="M21 12a9 9 0 1 1-6.22-8.56"/>',
  'log-out':
    '<path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4"/><polyline points="16 17 21 12 16 7"/><line x1="21" y1="12" x2="9" y2="12"/>',
  mail: '<rect x="2" y="5" width="20" height="14" rx="2"/><path d="m2 8 10 6 10-6"/>',
  menu: '<line x1="4" y1="7" x2="20" y2="7"/><line x1="4" y1="12" x2="20" y2="12"/><line x1="4" y1="17" x2="20" y2="17"/>',
  pencil: '<path d="M17 3a2.83 2.83 0 0 1 4 4L7.5 20.5 2 22l1.5-5.5Z"/>',
  phone:
    '<path d="M22 16.92v3a2 2 0 0 1-2.18 2 19.79 19.79 0 0 1-8.63-3.07 19.5 19.5 0 0 1-6-6A19.79 19.79 0 0 1 2.12 4.18 2 2 0 0 1 4.11 2h3a2 2 0 0 1 2 1.72c.13.96.36 1.9.7 2.81a2 2 0 0 1-.45 2.11L8.09 9.91a16 16 0 0 0 6 6l1.27-1.27a2 2 0 0 1 2.11-.45c.91.34 1.85.57 2.81.7A2 2 0 0 1 22 16.92Z"/>',
  plus: '<path d="M5 12h14"/><path d="M12 5v14"/>',
  power: '<path d="M12 2v9"/><path d="M18.4 6.6a9 9 0 1 1-12.8-.04"/>',
  'power-off': '<circle cx="12" cy="12" r="9"/><path d="m9 9 6 6"/><path d="m15 9-6 6"/>',
  presentation:
    '<path d="M2 3h20"/><path d="M21 3v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V3"/><path d="m7 21 5-5 5 5"/>',
  'refresh-cw':
    '<path d="M3 12a9 9 0 0 1 9-9 9.75 9.75 0 0 1 6.74 2.74L21 8"/><path d="M21 3v5h-5"/><path d="M21 12a9 9 0 0 1-9 9 9.75 9.75 0 0 1-6.74-2.74L3 16"/><path d="M3 21v-5h5"/>',
  save: '<path d="M19 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h11l5 5v11a2 2 0 0 1-2 2Z"/><polyline points="17 21 17 13 7 13 7 21"/><polyline points="7 3 7 8 15 8"/>',
  school:
    '<path d="M2 21h20"/><path d="M4 21V8l8-5 8 5v13"/><path d="m9 21 0-6h6v6"/><path d="M9 9h.01"/><path d="M15 9h.01"/>',
  search: '<circle cx="11" cy="11" r="7"/><line x1="21" y1="21" x2="16.5" y2="16.5"/>',
  'shield-check': '<path d="M12 22s8-3 8-9V5l-8-3-8 3v8c0 6 8 9 8 9Z"/><path d="m9 11.5 2 2 4-4"/>',
  'triangle-alert':
    '<path d="m21.73 18-8-14a2 2 0 0 0-3.46 0l-8 14A2 2 0 0 0 4.06 21h15.88a2 2 0 0 0 1.79-3Z"/><path d="M12 9v4"/><path d="M12 17h.01"/>',
  'user-cog':
    '<circle cx="12" cy="12" r="3"/><path d="M19.4 15a1.7 1.7 0 0 0 .33 1.82l.06.06a2 2 0 1 1-2.83 2.83l-.06-.06a1.7 1.7 0 0 0-1.82-.33 1.7 1.7 0 0 0-1 1.51V21a2 2 0 1 1-4 0v-.09a1.7 1.7 0 0 0-1-1.51 1.7 1.7 0 0 0-1.82.33l-.06.06a2 2 0 1 1-2.83-2.83l.06-.06a1.7 1.7 0 0 0 .33-1.82 1.7 1.7 0 0 0-1.51-1H3a2 2 0 1 1 0-4h.09a1.7 1.7 0 0 0 1.51-1 1.7 1.7 0 0 0-.33-1.82l-.06-.06a2 2 0 1 1 2.83-2.83l.06.06a1.7 1.7 0 0 0 1.82.33h.01a1.7 1.7 0 0 0 1-1.51V3a2 2 0 1 1 4 0v.09a1.7 1.7 0 0 0 1 1.51 1.7 1.7 0 0 0 1.82-.33l.06-.06a2 2 0 1 1 2.83 2.83l-.06.06a1.7 1.7 0 0 0-.33 1.82v.01a1.7 1.7 0 0 0 1.51 1H21a2 2 0 1 1 0 4h-.09a1.7 1.7 0 0 0-1.51 1Z"/>',
  'user-plus':
    '<circle cx="10" cy="8" r="4"/><path d="M2 21a8 8 0 0 1 13.6-5.1"/><path d="M19 11v6"/><path d="M16 14h6"/>',
  'user-round': '<circle cx="12" cy="8" r="4"/><path d="M5 21a7 7 0 0 1 14 0"/>',
  users:
    '<path d="M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M22 21v-2a4 4 0 0 0-3-3.87"/><path d="M16 3.13a4 4 0 0 1 0 7.75"/>',
};

export const STROKE_WIDTH = 1.5;

export function buildIconSpriteDefs(): string {
  return Object.entries(ICONS)
    .map(
      ([name, body]) =>
        `<symbol id="icon-${name}" viewBox="0 0 24 24"><g fill="none" stroke="currentColor" stroke-width="${STROKE_WIDTH}" stroke-linecap="round" stroke-linejoin="round">${body}</g></symbol>`,
    )
    .join('');
}
