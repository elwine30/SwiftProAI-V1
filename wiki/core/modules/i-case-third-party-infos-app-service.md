---
title: "ICaseThirdPartyInfosAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Registration/ICaseThirdPartyInfosAppService.cs"
updated: 2026-06-12
---

# ICaseThirdPartyInfosAppService

Contract for recording and managing third-party persons involved in an insurance incident, including file attachment removal.

## Public interface

Task<PagedResultDto<GetCaseThirdPartyInfoForViewDto>> GetAll(GetAllCaseThirdPartyInfosInput input)
Task<PagedResultDto<GetCaseThirdPartyInfoForViewDto>> GetAllForView(GetAllCaseThirdPartyInfosInput input)
Task<GetCaseThirdPartyInfoForEditOutput> GetCaseThirdPartyInfoForEdit(EntityDto input)
Task<bool?> CreateOrEdit(CreateOrEditCaseThirdPartyInfoDto input)
Task Delete(EntityDto input)
Task RemoveTpiFile(RemoveFile input)
Task<List<CaseThirdPartyInfoMainRegistrationLookupTableDto>> GetAllMainRegistrationForTableDropdown()

## External dependencies

- Abp.Core
