---
title: "UsersModule"
type: module
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/app/admin/users/users.module.ts"
updated: 2026-06-12
---

# UsersModule

Lazy-loaded feature module that assembles the user management screen, bundling the create/edit modal, permissions modal, Excel export modal, and staff modal together with the dynamic property manager.

## Dependencies

- [[users-component]] paginated user management list
- [[create-or-edit-user-modal-component]] modal form for creating or editing a user
- [[edit-user-permissions-modal-component]] modal for editing user-level permission overrides
- [[impersonation-service]] handles browser redirect for impersonating a user
- [[admin-shared-module]] shared admin UI primitives (permission tree, combos, etc.)

## External dependencies

- @angular/core

## Notes

CreateOrEditStaffModalComponent from a sibling 'common/staffs' path is declared here, making the user creation flow tightly coupled to staff creation.
