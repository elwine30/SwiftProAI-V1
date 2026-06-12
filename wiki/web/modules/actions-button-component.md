---
title: "ActionsButtonComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/shared/common/registration/actions-button.component.ts"
updated: 2026-06-12
---

# ActionsButtonComponent

Toolbar button strip shared across all registration wizard steps, wiring quick-actions such as adding expenses, declaration form, remarks and PDF export.

## Public interface

- `registerId: string`
- `fileRefNo: string`
- `goToAddExpense(): void`
- `goToAddClaims(): void`
- `openDeclarationForm(): void`
- `downloadRegistration(): void`
- `goToFileOrganizer(): void`
- `createNewRemark(): void`

## Dependencies

- [[step-nav-service]] provides registerId and viewOnly flag for routing decisions

## Used by

- [[app-common-module]]

## External dependencies

- `@angular/core`
- `@angular/router`

## Notes

Uses NavigationService.viewOnly to switch between the editable and read-only declaration modal. Depends on four child modals accessed via @ViewChild.
