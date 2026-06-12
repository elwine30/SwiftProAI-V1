---
title: "CaseAdjustersAppService"
type: service
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application/Registration/CaseAdjustersAppService.cs"
updated: 2026-06-12
---

# CaseAdjustersAppService

Manages the adjuster details sub-record for a case, including scope assignment, completion dates, extension remarks and read-only viewing for third-party organisations.

## Public interface

Task<PagedResultDto<GetCaseAdjusterForViewDto>> GetAll(GetAllCaseAdjustersInput input)
Task<GetCaseAdjusterForViewDto> GetCaseAdjusterForView(int registerId)
Task<GetCaseAdjusterForEditOutput> GetCaseAdjusterForEdit(EntityDto input)
Task CreateOrEdit(CreateOrEditCaseAdjusterDto input)
Task<List<CaseAdjusterScopeAssignmentLookupTableDto>> GetAllScopeAssignmentForTableDropdown()
Task<List<CaseAdjusterUserLookupTableDto>> GetAllEditorUserForTableDropdown()
Task<List<CaseAdjusterLookupTableDto>> GetAllCaseTypeForTableDropdown()
Task<List<CaseAdjusterLocationLookupTableDto>> GetAllStateLocationForTableDropdown(int parentId)

## Dependencies

- [[thinkn-insur-tech-app-service-base]] base class providing user, tenant and identity helpers

## External dependencies

- Abp.Zero

## Notes

When status code 5 is set, the parent MainRegistration's StatusId is updated synchronously. Scope assignments sort 'others' last. Third-party view enforces ViewThirdPartyCases access check by verifying AssignedOUId matches the current user's OU.
