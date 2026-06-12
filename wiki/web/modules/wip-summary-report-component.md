---
title: "WipSummaryReportComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/main/reports/wipSummaryReports/wipSummaryReport.component.ts"
updated: 2026-06-12
---

# WipSummaryReportComponent

Pivot-style WIP summary report filtered by case type, adjuster and insurance company, with Excel download and in-page pivot table.

## Public interface

search(): void — fetches pivot data if at least one filter selected
download(): void — exports to Excel if at least one filter selected
processPivotData(data): void
resetFilters(): void
getCellData(column, row): number

## External dependencies

- @angular/core
- lodash-es

## Notes

Injects services via injector.get() in the constructor rather than through constructor parameters — an uncommon ABP pattern that avoids declaring them in the constructor signature but reduces visibility. Guards search/download with a guard clause requiring at least one filter value.
