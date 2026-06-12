---
title: "CreateOrEditRoleModalComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/admin/roles/create-or-edit-role-modal.component.ts"
updated: 2026-06-12
---

# CreateOrEditRoleModalComponent

Modal form for creating or updating a role and its granted permissions via the embedded PermissionTreeComponent, with an unsaved-changes SweetAlert guard on close.

## Public interface

show(roleId?: number): void
save(): void
close(): void
modalSave: EventEmitter<any>

## Dependencies

- [[permission-tree-component]] embedded permission hierarchy tree for selecting granted permissions

## Used by

- [[roles-component]]

## External dependencies

- @angular/core
- @angular/forms
- ngx-bootstrap/modal
- rxjs/operators
- sweetalert2

## Notes

Uses sweetalert2 (swal) directly for the dirty-form confirmation dialog — this is a non-standard pattern versus the ABP abp.message.confirm used elsewhere in the codebase.
