---
title: "ScriptLoaderService"
type: service
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/shared/utils/script-loader.service.ts"
updated: 2026-06-12
---

# ScriptLoaderService

Dynamically injects JavaScript files into the document head at runtime, returning a Promise that resolves once each script has loaded.

## Public interface

- `load(...scripts: string[]): Promise<any>`
- `loadScript(name: string): Promise<any>`

## Used by

- [[utils-module]]

## External dependencies

- `@angular/core`

## Notes

Includes a legacy IE readyState code path alongside the standard onload path. onerror resolves rather than rejects — load failures are silently swallowed.
