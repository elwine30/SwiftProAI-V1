---
title: "CaseThirdPartyInfosAppService"
type: service
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application/Registration/CaseThirdPartyInfosAppService.cs"
updated: 2026-06-12
---

# CaseThirdPartyInfosAppService

Manages detailed third-party injury records per case, including automatic folder creation for document uploads, file persistence via FileOrgService, and cross-validation of identity numbers against police report data.

## Public interface

Task<PagedResultDto<GetCaseThirdPartyInfoForViewDto>> GetAll(GetAllCaseThirdPartyInfosInput input)
Task<PagedResultDto<GetCaseThirdPartyInfoForViewDto>> GetAllForView(GetAllCaseThirdPartyInfosInput input)
Task<GetCaseThirdPartyInfoForEditOutput> GetCaseThirdPartyInfoForEdit(EntityDto input)
Task<GetCaseThirdPartyInfoForViewDto> GetThirdPartyInfoForView(int id)
Task<bool?> CreateOrEdit(CreateOrEditCaseThirdPartyInfoDto input)
Task Delete(EntityDto input)
Task RemoveTpiFile(RemoveFile input)

## Dependencies

- [[thinkn-insur-tech-app-service-base]] base class providing user, tenant and identity helpers

## External dependencies

- Abp.Zero

## Notes

Returns a nullable bool from CreateOrEdit to signal IC inconsistency with police report data. Creates 8 typed folders (THP-EMP, THP-DET, THP-DL-DET-B, etc.) on first save. Changing an identity number triggers a folder rename via ChangeDirectory. Date range validation enforced server-side.
