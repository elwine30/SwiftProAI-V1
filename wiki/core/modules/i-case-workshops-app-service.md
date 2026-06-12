---
title: "ICaseWorkshopsAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Registration/ICaseWorkshopsAppService.cs"
updated: 2026-06-12
---

# ICaseWorkshopsAppService

Contract for assigning and managing vehicle repair workshops linked to an insurance case.

## Public interface

Task<PagedResultDto<GetCaseWorkshopForViewDto>> GetAll(GetAllCaseWorkshopsInput input)
Task<GetCaseWorkshopForEditOutput> GetCaseWorkshopForEdit(EntityDto input)
Task CreateOrEdit(CreateOrEditCaseWorkshopDto input)
Task Delete(EntityDto input)
Task<PagedResultDto<CaseWorkshopMainRegistrationLookupTableDto>> GetAllMainRegistrationForLookupTable(GetAllForLookupTableInput input)
Task<List<CaseWorkshopWorkshopLookupTableDto>> GetAllWorkshopForTableDropdown()
Task<GetCaseWorkshopForViewDto> GetCaseWorkshopForView(EntityDto input)

## Used by

- [[case-workshops-app-service]]

## External dependencies

- Abp.Core
