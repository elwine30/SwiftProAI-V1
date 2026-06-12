---
title: "StepNavComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/shared/common/registration/step-nav.component.ts"
updated: 2026-06-12
---

# StepNavComponent

Renders the 8-step wizard progress bar for the registration form and navigates between steps while toggling between create/edit and view-only URL patterns.

## Public interface

- `steps: array of step definitions with label, url and queryParams`
- `navigateToStep(selectedStepUrl?): void`
- `shouldShowMenu(params): boolean`
- `updateStepUrls(): void`
- `isViewMode(): boolean`

## Dependencies

- [[step-nav-service]] provides the current registerId and computed step URLs

## Used by

- [[app-common-module]]

## External dependencies

- `@angular/core`
- `@angular/router`

## Notes

Visibility is controlled by presence of stepId query param. The component swaps createOrEdit vs view segments in step URLs when mode changes.
