---
title: "CreateRegistrationModalComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/main/dashboard/create-registration-modal.component.ts"
updated: 2026-06-12
---

# CreateRegistrationModalComponent

Modal dialog for creating a new case registration from the dashboard, loading company, case-type and branch dropdowns on open.

## Public interface

@Output() modalSave: EventEmitter<any>
show(): void
close(): void
save(): void
updateAdjusterslist(branchId): void

## Used by

- [[dashboard-component]]

## External dependencies

- @angular/core
- ngx-bootstrap/modal
- rxjs/operators

## Notes

This modal is no longer navigated to from DashboardComponent.createNewRegistration() — that method now goes directly to /registration. Modal appears to be retained but bypassed.
