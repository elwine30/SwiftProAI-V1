---
title: "WIPReportsAppService"
type: service
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application/Reports/WIPReportsAppService.cs"
updated: 2026-06-12
---

# WIPReportsAppService

Provides a work-in-progress report of open cases with aging, due dates and insurer or lawyer references, filterable by adjuster, group, company and date range with Excel export.

## Public interface

Task<PagedResultDto<GetWIPReportForViewDto>> GetAll(GetAllWIPReportsInput input)
Task<FileDto> GetWIPReportsToExcel(GetAllWIPReportsForExcelInput input)

## Dependencies

- [[thinkn-insur-tech-app-service-base]] base class providing user, tenant and identity helpers

## External dependencies

- Abp.Zero

## Notes

Excludes status 4 (Completed Invoices) and 5 (Cancelled). Due date is hardcoded as AssignTime + 14 days. AgingDays is computed at query time using DateTime.Now. Query logic is duplicated between the two methods.
