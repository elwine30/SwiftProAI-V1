---
title: "DateRangePickerLuxonModifierDirective"
type: class
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/shared/utils/date-time/date-range-picker-luxon-modifier.directive.ts"
updated: 2026-06-12
---

# DateRangePickerLuxonModifierDirective

Adapter directive that intercepts ngx-bootstrap date-range picker value changes and emits a [DateTime, DateTime] tuple with timezone handling.

## Public interface

- `@Input() date`
- `@Output() dateChange: EventEmitter`

## Used by

- [[utils-module]]

## External dependencies

- `@angular/core`
- `ngx-bootstrap/datepicker`
- `rxjs`
- `rxjs/operators`
- `luxon`
- `just-compare`

## Notes

Filters out incomplete selections (where only one date is chosen) before emitting. Clears hours/minutes/seconds on both dates since the range picker has no time-selection capability.
