---
title: "DataService"
type: service
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/app/shared/common/data-service/data-service.ts"
updated: 2026-06-12
---

# DataService

Minimal singleton bag for passing temporary onboarding data between navigation steps where route params or query params are impractical.

## Public interface

- `setNotOnboardData(data: any): void`
- `getNotOnboardData(): any`

## External dependencies

- `@angular/core`

## Notes

Untyped data property — carries no schema. Acts as a cross-route transfer object specifically for the OU onboarding flow.
