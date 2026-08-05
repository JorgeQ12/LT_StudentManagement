import { HttpErrorResponse } from '@angular/common/http';

import { ApiProblemDetails, ApiValidationIssue } from './api.models';

const FALLBACK_PROBLEM: ApiProblemDetails = {
  title: 'No pudimos completar la solicitud',
  status: 0,
  detail: 'Verifica tu conexión e inténtalo nuevamente.',
};

export function problemFromError(error: unknown): ApiProblemDetails {
  if (!(error instanceof HttpErrorResponse) || !isProblemDetails(error.error)) {
    return FALLBACK_PROBLEM;
  }

  return error.error;
}

export function problemMessage(error: unknown): string {
  const problem = problemFromError(error);
  return problem.detail || problem.title;
}

export function fieldError(problem: ApiProblemDetails | null, fieldName: string): string | null {
  if (!problem?.errors) {
    return null;
  }

  const entry = Object.entries(problem.errors).find(
    ([key]) => key.toLocaleLowerCase() === fieldName.toLocaleLowerCase(),
  );
  const issue = entry?.[1][0];

  return typeof issue === 'string' ? issue : issue?.message || null;
}

function isProblemDetails(value: unknown): value is ApiProblemDetails {
  if (typeof value !== 'object' || value === null) {
    return false;
  }

  const candidate = value as Partial<ApiProblemDetails>;
  return typeof candidate.title === 'string' && typeof candidate.status === 'number';
}

export function toValidationIssue(message: string, code?: string): ApiValidationIssue {
  return { message, ...(code ? { code } : {}) };
}
