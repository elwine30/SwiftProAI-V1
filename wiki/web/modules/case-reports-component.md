---
title: "CaseReportsComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/main/reports/caseReports/caseReports.component.ts"
updated: 2026-06-12
---

# CaseReportsComponent

Pivot-table report component for case statistics, with date-range validation, Excel export, and dynamic column/row headers built from API response.

## Public interface

getCaseReports(event?): void
exportToExcel(): void
validateDateRange(): void — enforces max 6-month range for month mode
searchReport(): void
resetFilters(): void
getCellData(row, column): number

## External dependencies

- @angular/core
- @angular/router
- @angular/common/http
- primeng/api
- primeng/table
- primeng/paginator
- primeng/fileupload
- abp-ng2-module
- luxon
- lodash-es

## Notes

Pivot data is manually flattened from a nested dictionary to an array for PrimeNG table binding. reportFilter defaults to 'insuranceCompany' and reportTypeFilter to 'caseDetails' when empty — these are magic strings matched by the backend.
