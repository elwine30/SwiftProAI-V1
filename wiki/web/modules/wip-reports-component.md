---
title: "WIPReportsComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/main/reports/wipReports/wipReports.component.ts"
updated: 2026-06-12
---

# WIPReportsComponent

Work-in-progress report with multi-dimension filters (company, adjuster, lawyer, group) and Excel import/export via PrimeNG FileUpload and a direct upload URL.

## Public interface

getWIPReports(event?): void
exportToExcel(): void
searchReport(): void
resetFilters(): void

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
- rxjs

## Notes

Constructs upload URL using AppConsts.remoteServiceBaseUrl + '/WIPReports/ImportFromExcel' — this bypasses the NSwag proxy layer, which is noted as a convention violation in CLAUDE.md.
