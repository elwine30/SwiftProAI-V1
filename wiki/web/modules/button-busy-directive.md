---
title: "ButtonBusyDirective"
type: class
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/shared/utils/button-busy.directive.ts"
updated: 2026-06-12
---

# ButtonBusyDirective

Directive that disables a button and replaces its content with a spinner and localised 'Processing...' text while an async operation is in progress.

## Public interface

- `@Input() set buttonBusy(isBusy: boolean)`
- `@Input() busyText: string`

## Used by

- [[utils-module]]

## External dependencies

- `@angular/core`

## Notes

Stores the original innerHTML to restore it when isBusy becomes false. Uses a _disabledBefore attribute guard to avoid re-enabling a button that was already disabled before the directive activated.
