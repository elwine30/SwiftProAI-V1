---
title: "IAuditEntriesAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Audit/IAuditEntriesAppService.cs"
updated: 2026-06-12
---

# IAuditEntriesAppService

Read-only contract for querying domain audit entries and exporting them to Excel.

## Public interface

Task<PagedResultDto<GetAuditEntryForViewDto>> GetAll(GetAllAuditEntriesInput input)
Task<FileDto> GetAuditEntriesToExcel(GetAllAuditEntriesForExcelInput input)

## External dependencies

- Abp.Core
