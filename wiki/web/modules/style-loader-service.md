---
title: "StyleLoaderService"
type: service
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/shared/utils/style-loader.service.ts"
updated: 2026-06-12
---

# StyleLoaderService

Dynamically injects CSS link elements into the document head at runtime, supporting both variadic and array input.

## Public interface

- `load(...styles: string[]): Promise<any>`
- `loadArray(styles: string[]): Promise<any>`
- `loadStyle(name: string): Promise<any>`

## Used by

- [[dynamic-resources-helper]]
- [[utils-module]]

## External dependencies

- `@angular/core`

## Notes

Used by DynamicResourcesHelper to load Metronic and PrimeNG CSS at boot. Same silent-failure pattern as ScriptLoaderService — onerror resolves with loaded: false.
