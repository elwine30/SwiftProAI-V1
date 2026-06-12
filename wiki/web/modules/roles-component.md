---
title: "RolesComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/admin/roles/roles.component.ts"
updated: 2026-06-12
---

# RolesComponent

Lists all application roles with permission-based filtering, supports create/edit via modal, delete with confirmation, and optionally shows entity history when the ABP EntityHistory feature is enabled.

## Public interface

getRoles(): void
createRole(): void
deleteRole(role: RoleListDto): void
showHistory(role: RoleListDto): void

## Dependencies

- [[create-or-edit-role-modal-component]] modal form for creating or updating a role and its permissions
- [[permission-tree-modal-component]] modal filter picker used for permission-based role search

## Used by

- [[permission-tree-modal-component]]

## External dependencies

- @angular/core
- primeng/table
- rxjs/operators
- lodash-es

## Notes

Entity history enabled state is read from `(abp as any).custom.EntityHistory` — a non-typed ABP global. The full entity type name is 'ThinknInsurTech.Authorization.Roles.Role'.
