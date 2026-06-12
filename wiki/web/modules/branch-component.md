---
title: "BranchComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/main/branches/branch/branch.component.ts"
updated: 2026-06-12
---

# BranchComponent

CRUD list page for office branches; wires modal events to reload the table on save.

## Public interface

getBranch(event?): void
createBranch(): void
editBranch(branch): void
viewBranch(branch): void
deleteBranch(branch): void
reloadPage(): void
resetFilters(): void

## Dependencies

- [[create-or-edit-branch-modal-component]] modal for creating and editing branch records
- [[view-branch-modal-component]] read-only view modal for branch details

## Used by

- [[branch-component]]

## External dependencies

- @angular/core
- @angular/router
- primeng/api
- primeng/table
- primeng/paginator
- abp-ng2-module
- luxon
- lodash-es

## Notes

Subscribes to createEditModal.modalSave in ngOnInit to trigger reload — an EventEmitter subscription pattern used consistently across list/modal pairs in this codebase.
