---
title: "NavigationService"
type: service
language: typescript
layer: application
path: "SwiftProAI.Web/angular/src/app/shared/common/registration/step-nav-service.ts"
updated: 2026-06-12
---

# NavigationService

Holds the current registration case ID and view-only flag, and computes the 8-step registration wizard URLs dynamically.

## Public interface

- `registerId: string`
- `viewOnly: boolean`
- `step1Url through step8Url: string (computed getters)`
- `getAction(): string` — returns 'view' or 'createOrEdit'

## Used by

- [[step-nav-component]]
- [[actions-button-component]]

## External dependencies

- `@angular/core`

## Notes

Registered as providedIn: 'root' singleton so that registerId set in one step component is visible in StepNavComponent and ActionsButtonComponent.
