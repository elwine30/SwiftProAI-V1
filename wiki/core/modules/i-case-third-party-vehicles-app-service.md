---
title: "ICaseThirdPartyVehiclesAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Registration/ICaseThirdPartyVehiclesAppService.cs"
updated: 2026-06-12
---

# ICaseThirdPartyVehiclesAppService

Contract for managing third-party vehicle records linked to an insurance case.

## Public interface

Task<PagedResultDto<GetCaseThirdPartyVehicleForViewDto>> GetAll(GetAllCaseThirdPartyVehiclesInput input)
Task<PagedResultDto<GetCaseThirdPartyVehicleForViewDto>> GetAllForView(GetAllCaseThirdPartyVehiclesInput input)
Task<GetCaseThirdPartyVehicleForEditOutput> GetCaseThirdPartyVehicleForEdit(EntityDto input)
Task CreateOrEdit(CreateOrEditCaseThirdPartyVehicleDto input)
Task Delete(EntityDto input)
Task<List<CaseThirdPartyVehicleMainRegistrationLookupTableDto>> GetAllMainRegistrationForTableDropdown()

## External dependencies

- Abp.Core
