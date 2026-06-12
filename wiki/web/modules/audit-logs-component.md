---
title: "AuditLogsComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/admin/audit-logs/audit-logs.component.ts"
updated: 2026-06-12
---

# AuditLogsComponent

Dual-paged view showing ABP framework audit logs (API calls with execution time and exception filters) and entity change history, both exportable to Excel.

## Public interface

getAuditLogs(event?: LazyLoadEvent): void
getEntityChanges(event?: LazyLoadEvent): void
showAuditLogDetails(record: AuditLogListDto): void
showEntityChangeDetails(record: EntityChangeListDto): void
exportToExcelAuditLogs(): void
exportToExcelEntityChanges(): void
truncateStringWithPostfix(text: string, length: number): string

## Dependencies

- [[audit-log-detail-modal-component]] modal for displaying full details of a single audit log entry

## Used by

- [[audit-log-detail-modal-component]]

## External dependencies

- @angular/core
- luxon
- primeng/api
- primeng/table
- primeng/paginator

## Notes

Instantiates two separate PrimengTableHelper instances for the two grids. Delegates truncation to `abp.utils.truncateStringWithPostfix` from the global ABP namespace.
