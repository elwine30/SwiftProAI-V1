---
title: "ICaseAdjustersAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Registration/ICaseAdjustersAppService.cs"
updated: 2026-06-12
---

# ICaseAdjustersAppService

Contract for assigning and managing adjuster records on a case, including location and scope-based lookup helpers.

## Public interface

Task<PagedResultDto<GetCaseAdjusterForViewDto>> GetAll(GetAllCaseAdjustersInput input)
Task<GetCaseAdjusterForViewDto> GetCaseAdjusterForView(int registerId)
Task<GetCaseAdjusterForEditOutput> GetCaseAdjusterForEdit(EntityDto input)
Task CreateOrEdit(CreateOrEditCaseAdjusterDto input)
Task<List<CaseAdjusterScopeAssignmentLookupTableDto>> GetAllScopeAssignmentForTableDropdown()
Task<List<CaseAdjusterLocationLookupTableDto>> GetAllStateLocationForTableDropdown(int parentId)
Task<List<CaseAdjusterUserLookupTableDto>> GetAllEditorUserForTableDropdown()
Task<List<CaseAdjusterLookupTableDto>> GetAllCaseTypeForTableDropdown()

## External dependencies

- Abp.Core
