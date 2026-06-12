---
title: "OnlyNumberDirective"
type: component
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/app/shared/common/input-types/only-number-input-type/only-number.directive.ts"
updated: 2026-06-12
---

# OnlyNumberDirective

ControlValueAccessor directive that restricts input fields to digits only, showing an inline validation error message for non-numeric key presses.

## Public interface

- `selector: [onlyNumber]`
- `writeValue(value): void`
- `registerOnChange(fn): void`
- `registerOnTouched(fn): void`
- `setDisabledState(isDisabled): void`

## Used by

- [[app-common-module]]

## External dependencies

- `@angular/core`
- `@angular/forms`

## Notes

Implements ControlValueAccessor and injects a DOM error element directly via Renderer2 — bypasses standard Angular form error patterns.
