---
title: "IScopeAssignmentsAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Registration/IScopeAssignmentsAppService.cs"
updated: 2026-06-12
---

# IScopeAssignmentsAppService

CRUD contract for adjuster scope assignment records that define which case types and regions an adjuster is authorised to handle.

## Public interface

Task<PagedResultDto<GetScopeAssignmentForViewDto>> GetAll(GetAllScopeAssignmentsInput input)
Task<GetScopeAssignmentForViewDto> GetScopeAssignmentForView(int id)
Task<GetScopeAssignmentForEditOutput> GetScopeAssignmentForEdit(EntityDto input)
Task CreateOrEdit(CreateOrEditScopeAssignmentDto input)
Task Delete(EntityDto input)

## External dependencies

- Abp.Core
