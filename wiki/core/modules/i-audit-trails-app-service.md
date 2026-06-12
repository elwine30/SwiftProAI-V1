---
title: "IAuditTrailsAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Audit/IAuditTrailsAppService.cs"
updated: 2026-06-12
---

# IAuditTrailsAppService

CRUD contract for custom audit trail records tracking case-level business events with Excel export support.

## Public interface

Task<PagedResultDto<GetAuditTrailForViewDto>> GetAll(GetAllAuditTrailsInput input)
Task<GetAuditTrailForViewDto> GetAuditTrailForView(int id)
Task<GetAuditTrailForEditOutput> GetAuditTrailForEdit(EntityDto input)
Task CreateOrEdit(CreateOrEditAuditTrailDto input)
Task Delete(EntityDto input)
Task<FileDto> GetAuditTrailsToExcel(GetAllAuditTrailsForExcelInput input)

## External dependencies

- Abp.Core

## Notes

Accompanied by IAuditTrailsAppServiceExtended (currently empty) per ABP RAD tool convention for customisation hooks.
