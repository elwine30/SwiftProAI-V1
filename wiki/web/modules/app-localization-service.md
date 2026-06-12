---
title: "AppLocalizationService"
type: service
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/app/shared/common/localization/app-localization.service.ts"
updated: 2026-06-12
---

# AppLocalizationService

Extends ABP's LocalizationService to bind the app's default localisation source name, exposing a convenience l() method used throughout all components.

## Public interface

- `l(key: string, ...args): string` — localise using default source
- `ls(sourceName: string, key: string, ...args): string` — localise with explicit source

## Used by

- [[app-common-module]]
- [[date-time-service]]

## External dependencies

- `@angular/core`
- `abp-ng2-module`

## Notes

All components that extend AppComponentBase receive this service indirectly through the injector. Uses abp.utils.formatString for argument interpolation.
