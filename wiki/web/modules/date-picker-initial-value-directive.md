---
title: "DatePickerInitialValueSetterDirective"
type: component
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/app/shared/common/timing/date-picker-initial-value.directive.ts"
updated: 2026-06-12
---

# DatePickerInitialValueSetterDirective

Structural directive that sets the displayed text of a Bootstrap datepicker input to a Luxon DateTime value after the view initialises.

## Public interface

- `@Input() ngModel: DateTime`

## External dependencies

- `@angular/core`

## Notes

Uses setTimeout to defer DOM write until after change detection completes; calls Luxon's toFormat('D') (locale-aware short date).
