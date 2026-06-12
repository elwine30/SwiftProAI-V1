---
title: "IBranchAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Branches/IBranchAppService.cs"
updated: 2026-06-12
---

# IBranchAppService

CRUD contract for branch office records used to associate cases and adjusters with geographic divisions.

## Public interface

Task<PagedResultDto<GetBranchForViewDto>> GetAll(GetAllBranchInput input)
Task<GetBranchForViewDto> GetBranchForView(int id)
Task<GetBranchForEditOutput> GetBranchForEdit(EntityDto input)
Task CreateOrEdit(CreateOrEditBranchDto input)
Task Delete(EntityDto input)

## Used by

- [[branch-app-service]]

## External dependencies

- Abp.Core
