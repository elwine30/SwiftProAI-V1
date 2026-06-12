---
title: "CaseReportsAppService"
type: service
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application/Reports/CaseReportsAppService.cs"
updated: 2026-06-12
---

# CaseReportsAppService

Generates a monthly cross-tabulation report of case counts grouped by insurance company, case type or state, with optional Excel export.

## Public interface

Task<GetCaseReportForViewDto> GetAll(GetAllCaseReportsInput input)
Task<FileDto> GetCaseReportsToExcel(GetAllCaseReportsForExcelInput input)

## Dependencies

- [[thinkn-insur-tech-app-service-base]] base class providing user, tenant and identity helpers

## External dependencies

- Abp.Zero

## Notes

Report data is a nested dictionary keyed by row label then month string. An invoiceReport mode filters to CompletedInvoices status only. Logic is duplicated between GetAll and GetCaseReportsToExcel — a TODO candidate.
