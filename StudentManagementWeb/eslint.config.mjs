// @ts-check
import eslint from '@eslint/js';
import { defineConfig } from 'eslint/config';
import angular from 'angular-eslint';
import boundaries from 'eslint-plugin-boundaries';
import tseslint from 'typescript-eslint';

export default defineConfig([
  {
    ignores: ['.angular/**', 'dist/**', 'node_modules/**'],
  },
  {
    files: ['**/*.ts'],
    extends: [
      eslint.configs.recommended,
      tseslint.configs.recommended,
      tseslint.configs.stylistic,
      angular.configs.tsRecommended,
    ],
    processor: angular.processInlineTemplates,
    rules: {
      '@angular-eslint/directive-selector': [
        'error',
        {
          type: 'attribute',
          prefix: 'sm',
          style: 'camelCase',
        },
      ],
      '@angular-eslint/component-selector': [
        'error',
        {
          type: 'element',
          prefix: 'sm',
          style: 'kebab-case',
        },
      ],
    },
  },
  {
    files: ['**/*.ts'],
    plugins: {
      boundaries,
    },
    settings: {
      'boundaries/elements': [
        { type: 'core', pattern: 'src/app/core/**/*' },
        { type: 'shared', pattern: 'src/app/shared/**/*' },
        { type: 'features', pattern: 'src/app/features/**/*' },
      ],
    },
    rules: {
      'boundaries/dependencies': [
        'error',
        {
          default: 'allow',
          policies: [
            {
              from: { element: { type: 'core' } },
              disallow: { to: { element: { type: 'features' } } },
            },
            {
              from: { element: { type: 'shared' } },
              disallow: { to: { element: { type: 'features' } } },
            },
            {
              from: { element: { type: 'shared' } },
              disallow: { to: { element: { type: 'core' } } },
            },
          ],
        },
      ],
    },
  },
  {
    files: ['**/*.html'],
    extends: [angular.configs.templateRecommended, angular.configs.templateAccessibility],
    rules: {},
  },
]);
