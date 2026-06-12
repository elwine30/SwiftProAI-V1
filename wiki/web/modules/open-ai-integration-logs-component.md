---
title: "OpenAIIntegrationLogsComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/admin/integration/openAIIntegrationLogs/openAIIntegrationLogs.component.ts"
updated: 2026-06-12
---

# OpenAIIntegrationLogsComponent

Admin page for browsing and filtering OpenAI API call logs, showing token usage (prompt, completion), cost ranges, case number, OU filter, and date range — providing visibility over AI consumption.

## Public interface

getOpenAIIntegrationLogs(event?: LazyLoadEvent): void
reloadPage(): void
resetFilters(): void
onDropdownChange(event: any): void

## Dependencies

- [[view-open-ai-integration-log-modal-component]] modal for viewing full details of a single log entry

## Used by

- [[view-open-ai-integration-log-modal-component]]

## External dependencies

- @angular/core
- @angular/router
- abp-ng2-module
- primeng/table
- primeng/paginator
- primeng/api
- luxon
- lodash-es

## Notes

Loads all OUs on ngOnInit via OrganizationUnitServiceProxy.getAll() — not paginated. Has console.log left in production code. Combines min/max token and cost filters with null-coalescing to empty sentinel values.
