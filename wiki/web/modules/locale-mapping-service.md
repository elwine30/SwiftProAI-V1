---
title: "LocaleMappingService"
type: class
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/shared/locale-mapping.service.ts"
updated: 2026-06-12
---

# LocaleMappingService

Maps locale codes between different naming conventions using the locale-mapping configuration stored in AppConsts.localeMappings.

## Public interface

- `map(mappingSource: string, locale: string): string`

## Dependencies

- [[app-consts]] provides the localeMappings configuration array

## External dependencies

- `lodash-es`
