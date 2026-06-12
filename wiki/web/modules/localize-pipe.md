---
title: "LocalizePipe"
type: class
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/shared/common/pipes/localize.pipe.ts"
updated: 2026-06-12
---

# LocalizePipe

Angular pipe that translates a localisation key to the current language using the ABP LocalizationService.

## Public interface

- `transform(key: string, ...args: any[]): string`

## Dependencies

- [[app-consts]] — application constants used during localisation resolution

## Used by

- [[utils-module]]

## External dependencies

- `@angular/core`
- `abp-ng2-module`

## Notes

Falls back to the key itself if no translation is found. Calls abp.utils.formatString for string interpolation with arguments.
