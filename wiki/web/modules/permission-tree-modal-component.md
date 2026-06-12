---
title: "PermissionTreeModalComponent"
type: component
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/app/admin/shared/permission-tree-modal.component.ts"
updated: 2026-06-12
---

# PermissionTreeModalComponent

Modal wrapper around PermissionTreeComponent used as a filter picker in list pages; loads all permissions on init and emits the selection count as a user-facing ABP notification on close.

## Public interface

@Input() dontAddOpenerButton: boolean
@Input() singleSelect: boolean
@Input() disableCascade: boolean
@Output() onModalclose: EventEmitter<string[]>
openPermissionTreeModal(): void
closePermissionTreeModal(): void
getSelectedPermissions(): string[]

## Dependencies

- [[permission-tree-component]] embedded permission hierarchy tree that drives the selection

## Used by

- [[users-component]]
- [[roles-component]]

## External dependencies

- @angular/core
- ngx-bootstrap/modal

## Notes

Calls `abp.notify.success` (global) rather than the typed this.notify helper from AppComponentBase.
