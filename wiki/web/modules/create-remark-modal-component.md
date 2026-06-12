---
title: "CreateRemarkModalComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/main/registration/create-remark-modal.component.ts"
updated: 2026-06-12
---

# CreateRemarkModalComponent

Shared modal for adding and viewing remarks on a case, with whitespace validation and DirtyFormGuard-compatible close logic.

## Public interface

@Output() modalSave: EventEmitter<any>
show(registerId): void
save(): void
close(): void
getRemarkDetails(event?): void

## External dependencies

- @angular/core
- @angular/forms
- ngx-bootstrap/modal
- primeng/paginator
- primeng/table
- primeng/api
- rxjs/operators

## Notes

Close() uses native confirm() rather than the ABP message service for the unsaved-changes prompt — inconsistent with the rest of the codebase which uses swalAlert.
