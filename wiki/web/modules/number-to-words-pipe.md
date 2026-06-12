---
title: "NumberToWordsPipe"
type: class
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/shared/common/pipes/number-to-words.pipe.ts"
updated: 2026-06-12
---

# NumberToWordsPipe

Angular pipe that converts numeric amounts to Malaysian Ringgit word representation for invoice and document generation.

## Public interface

- `transform(value: any): string`

## Used by

- [[utils-module]]

## External dependencies

- `@angular/core`

## Notes

Domain-specific to Malaysian currency (RINGGIT/CENTS). Handles both integer and floating-point values, padding cents to two digits.
