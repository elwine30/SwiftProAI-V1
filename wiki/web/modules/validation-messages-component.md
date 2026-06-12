---
title: "ValidationMessagesComponent"
type: component
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/shared/utils/validation-messages.component.ts"
updated: 2026-06-12
---

# ValidationMessagesComponent

Reusable component that renders localised validation error messages for a given Angular form control.

## Public interface

- `@Input() formCtrl` — the NgControl to observe
- `@Input() set errorDefs(value: ErrorDef[])` — custom error definitions

## Used by

- [[utils-module]]

## External dependencies

- `@angular/core`
- `@angular/forms`
- `lodash-es`

## Notes

Merges a built-in set of standard error definitions (required, minlength, maxlength, email, min, max, pattern) with caller-supplied overrides. Errors only display after the control is dirty or touched.
