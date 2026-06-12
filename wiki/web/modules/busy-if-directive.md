---
title: "BusyIfDirective"
type: class
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/shared/utils/busy-if.directive.ts"
updated: 2026-06-12
---

# BusyIfDirective

Directive that shows an inline NgxSpinner overlay on the host element while a bound boolean is true.

## Public interface

- `@Input() busyIf: boolean`

## Used by

- [[utils-module]]

## External dependencies

- `@angular/core`
- `ngx-spinner`

## Notes

Creates an NgxSpinnerComponent via ComponentFactoryResolver (pre-Ivy dynamic component API). Generates a random spinner name per instance to support multiple concurrent spinners. The show/hide has a 1-second delay to avoid flicker.
