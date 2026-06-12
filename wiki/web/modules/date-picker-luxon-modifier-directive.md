---
title: "DatePickerLuxonModifierDirective"
type: class
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/shared/utils/date-time/date-picker-luxon-modifier.directive.ts"
updated: 2026-06-12
---

# DatePickerLuxonModifierDirective

Adapter directive that intercepts ngx-bootstrap datepicker value changes and emits Luxon DateTime objects instead of native JS Date objects, handling timezone conversion.

## Public interface

- `@Input() date`
- `@Input() isUtc: boolean`
- `@Input() withTimepicker: boolean`
- `@Output() dateChange: EventEmitter`

## Used by

- [[utils-module]]

## External dependencies

- `@angular/core`
- `ngx-bootstrap/datepicker`
- `rxjs`
- `luxon`
- `just-compare`

## Notes

Checks abp.clock.provider.supportsMultipleTimezone to decide whether to apply IANA timezone conversion via DateTimeService. Uses @Self() injection to bind to the co-located BsDatepickerDirective instance.
