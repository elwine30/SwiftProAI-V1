---
title: "AuditTrailsComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/admin/audit/auditTrails/auditTrails.component.ts"
updated: 2026-06-12
---

# AuditTrailsComponent

Domain-specific audit trail viewer (distinct from ABP audit logs) showing entity-level change entries from AuditEntriesServiceProxy, filterable by table name, organisation unit and changed-by user.

## Public interface

getAuditTrails(event?: LazyLoadEvent): void
reloadPage(): void
exportToExcel(): void
resetFilters(): void

## External dependencies

- @angular/core
- @angular/router
- abp-ng2-module
- primeng/table
- primeng/paginator
- primeng/api
- luxon

## Notes

Uses two service proxies: AuditTrailsServiceProxy (injected but not called — likely residual) and AuditEntriesServiceProxy (the active one). The create/edit and view child view references are commented out.
