---
title: "ILawFirmsAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/LawFirms/ILawFirmsAppService.cs"
updated: 2026-06-12
---

# ILawFirmsAppService

CRUD contract for legal firm records referenced when assigning lawyers to insurance cases.

## Public interface

Task<PagedResultDto<GetLawFirmForViewDto>> GetAll(GetAllLawFirmsInput input)
Task<GetLawFirmForViewDto> GetLawFirmForView(int id)
Task<GetLawFirmForEditOutput> GetLawFirmForEdit(EntityDto input)
Task CreateOrEdit(CreateOrEditLawFirmDto input)
Task Delete(EntityDto input)

## Used by

- [[law-firms-app-service]]

## External dependencies

- Abp.Core
