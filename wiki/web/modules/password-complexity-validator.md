---
title: "PasswordComplexityValidator"
type: class
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/shared/utils/validation/password-complexity-validator.directive.ts"
updated: 2026-06-12
---

# PasswordComplexityValidator

Custom Angular validator directive that enforces configurable password complexity rules (digit, upper, lower, non-alphanumeric, minimum length) driven by ABP settings.

## Public interface

- `@Input() requireDigit: boolean`
- `@Input() requireUppercase: boolean`
- `@Input() requireLowercase: boolean`
- `@Input() requireNonAlphanumeric: boolean`
- `@Input() requiredLength: number`
- `validate(control: AbstractControl): ValidationErrors | null`

## Used by

- [[utils-module]]

## External dependencies

- `@angular/core`
- `@angular/forms`
