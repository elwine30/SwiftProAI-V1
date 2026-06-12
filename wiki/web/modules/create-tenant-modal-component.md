---
title: "CreateTenantModalComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/admin/tenants/create-tenant-modal.component.ts"
updated: 2026-06-12
---

# CreateTenantModalComponent

Modal form for provisioning a new tenant, dynamically showing or hiding subscription end-date and trial fields based on the selected edition's pricing tier.

## Public interface

show(): void
save(): void
close(): void
onEditionChange(): void
modalSave: EventEmitter<any>

## Used by

- [[tenants-component]]

## External dependencies

- @angular/core
- ngx-bootstrap/modal
- rxjs/operators
- lodash-es
- luxon

## Notes

Password complexity setting is fetched on show() to validate the initial admin password. The edition combo populates from CommonLookupServiceProxy.getEditionsForCombobox — shared with other modals.
