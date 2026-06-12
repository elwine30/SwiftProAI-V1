---
title: "AdjusterReportsComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/main/reports/adjusterReports/adjusterReports.component.ts"
updated: 2026-06-12
---

# AdjusterReportsComponent

Adjuster productivity report filterable by month, year and individual adjuster, with Excel export.

## Public interface

getAdjusterReports(event?): void
exportToExcel(): void
onMonthFilterChange(): void — syncs yearFilter to monthFilter
onYearFilterChange(): void — resets monthFilter to avoid conflicts
resetFilters(): void

## External dependencies

- @angular/core
- primeng/api
- primeng/table
- primeng/paginator
- ngx-bootstrap/datepicker
- luxon
- lodash-es

## Notes

Uses ngx-bootstrap datepicker in month-only and year-only minMode rather than PrimeNG — consistent with the rest of the main module datepicker configuration.
