---
title: "CreateOrEditUserModalComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/admin/users/create-or-edit-user-modal.component.ts"
updated: 2026-06-12
---

# CreateOrEditUserModalComponent

Modal form for creating or editing a user, loading password complexity rules, role assignments, and profile picture, then submitting via CreateOrUpdateUserInput.

## Public interface

show(userId?: number): void
save(): void
close(): void
modalSave: EventEmitter<any>
getAssignedRoleCount(): number

## Dependencies

- [[users-data-service]] event bus used to signal the embedded staff sub-modal on show and save

## Used by

- [[users-component]]
- [[users-module]]

## External dependencies

- @angular/core
- ngx-bootstrap/modal
- rxjs/operators
- lodash-es

## Notes

Organisation unit tree assignment is commented out throughout — dead code with TODO comments. Calls UsersDataService.triggerShow/triggerSave to coordinate with the embedded CreateOrEditStaffModalComponent.
