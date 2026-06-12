---
title: "EqualValidator"
type: class
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/shared/utils/validation/equal-validator.directive.ts"
updated: 2026-06-12
---

# EqualValidator

Custom Angular validator directive that checks whether two form controls have matching values, supporting both normal and reverse validation modes for password-confirmation fields.

## Public interface

- `validate(control: AbstractControl): ValidationErrors | null`

## Used by

- [[utils-module]]

## External dependencies

- `@angular/core`
- `@angular/forms`

## Notes

The reverse='true' mode sets the error on the partner control rather than the current control, useful when the confirmation field should trigger validation on the original password field.
