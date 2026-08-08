export interface CatalogAcademicProgram {
  readonly id: string;
  readonly code: string;
  readonly name: string;
  readonly description: string;
  readonly status: 'Active' | 'Inactive';
}

export interface CatalogCourse {
  readonly id: string;
  readonly academicProgramId: string;
  readonly code: string;
  readonly name: string;
  readonly credits: number;
  readonly status: 'Active' | 'Inactive';
  readonly professorId: string | null;
  readonly professorFullName: string | null;
}
