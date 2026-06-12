---
title: "AdjusterReportsAppService"
type: service
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application/Reports/AdjusterReportsAppService.cs"
updated: 2026-06-12
---

# AdjusterReportsAppService

Provides a paginated report of completed-invoice cases per adjuster including service fee, insurance reference and Excel export capability.

## Public interface

Task<PagedResultDto<GetAdjusterReportForViewDto>> GetAll(GetAllAdjusterReportsInput input)
Task<FileDto> GetAdjusterReportsToExcel(GetAllAdjusterReportsForExcelInput input)

## Dependencies

- [[thinkn-insur-tech-app-service-base]] base class providing user, tenant and identity helpers

## External dependencies

- Abp.Zero

## Notes

Filters to CompletedInvoices status only. Optional UserIdFilter allows per-adjuster scoping. Requires Pages_AdjusterReports permission.
