---
title: "UsersDataService"
type: service
language: typescript
layer: application
path: "SwiftProAI.Web/angular/src/app/admin/users/usersDataService.ts"
updated: 2026-06-12
---

# UsersDataService

Lightweight event bus that lets the CreateOrEditUserModal signal the staff sub-modal when a user is shown or saved, decoupling the two components without direct @ViewChild coupling.

## Public interface

onSave: EventEmitter<any>
onShow: EventEmitter<any>
triggerShow(userId?: number): void
triggerSave(userId?: number): void

## Used by

- [[users-component]]
- [[create-or-edit-user-modal-component]]

## External dependencies

- @angular/core

## Notes

Provided in root — note the filename uses camelCase (usersDataService.ts) while the class name uses PascalCase (UsersDataService), inconsistent with the project naming convention.
