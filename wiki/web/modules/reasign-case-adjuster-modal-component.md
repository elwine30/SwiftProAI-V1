---
title: "ReasignCaseAdjusterModalComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/main/dashboard/reasign-adjuster-modal.component.ts"
updated: 2026-06-12
---

# ReasignCaseAdjusterModalComponent

Modal for reassigning the adjuster on an existing case, fetching the current registration data and recording a remark on save.

## Public interface

@Output() modalSave: EventEmitter<any>
show(registerId): void
save(): void
close(): void

## Used by

- [[dashboard-component]]

## External dependencies

- @angular/core
- ngx-bootstrap/modal
- rxjs/operators

## Notes

Loads adjuster dropdown scoped to the current case branch. Bundles a RemarkDto inside ReassignCaseAdjusterDto to log the reason for reassignment.
