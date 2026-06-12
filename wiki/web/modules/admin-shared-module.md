---
title: "AdminSharedModule"
type: module
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/app/admin/shared/admin-shared.module.ts"
updated: 2026-06-12
---

# AdminSharedModule

Barrel module that declares and re-exports all reusable admin UI primitives (permission tree, role combo, feature tree, edition combo, OU tree) along with their PrimeNG and ngx-bootstrap dependencies.

## Public interface

RoleComboComponent
PermissionTreeComponent
PermissionTreeModalComponent
PermissionComboComponent
OrganizationUnitsTreeComponent
FeatureTreeComponent
EditionComboComponent

## Dependencies

- [[permission-tree]] reusable PrimeNG tree component for rendering permission hierarchies
- [[permission-tree-modal]] modal wrapper around the permission tree for filter pickers
- [[permission-combo]] dropdown combo for selecting a single permission
- [[organization-unit-tree]] interactive OU tree with drag-and-drop re-parenting
- [[feature-tree]] tree component for managing ABP feature flags per edition/tenant
- [[edition-combo]] dropdown combo for selecting a subscription edition
- [[role-combo]] dropdown combo for selecting an application role

## Used by

- [[users-module]]

## External dependencies

- @angular/core
- @angular/forms
- primeng/tree
- primeng/table
- primeng/dropdown
- primeng/autocomplete
- primeng/editor
- primeng/inputmask
- ngx-bootstrap/tooltip
- ngx-image-cropper
- angular-imask
- @craftsjs/perfect-scrollbar
- @awaismirza/angular2-counto
- ng2-file-upload
- primeng/fileupload
