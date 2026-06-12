---
title: "RegistrationComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/main/registration/registration.component.ts"
updated: 2026-06-12
---

# RegistrationComponent

Step-1 registration form for creating a new case, with duplicate-vehicle-check table and DirtyFormGuard protection against accidental navigation.

## Public interface

@Output() formSave: EventEmitter<any>
save(): void — calls createMainRegistration and stores returned registerId
proceedToRegistrationDetails(): void — navigates to caseAdjuster step
backToDashboard(): void
getDuplicateRegistration(): void — searches existing cases by vehicle number
updateAdjusterslist(branchId): void
canDeactivate(): Promise<boolean>|boolean

## External dependencies

- @angular/core
- @angular/forms
- @angular/router
- primeng/api
- primeng/table
- primeng/paginator
- rxjs/operators
- luxon

## Notes

Implements DirtyFormGuard; uses swalAlert confirm dialog pattern. Sets default values to 0 rather than undefined before sending to API to avoid backend null errors.
