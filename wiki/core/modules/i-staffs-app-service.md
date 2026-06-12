---
title: "IStaffsAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Common/IStaffsAppService.cs"
updated: 2026-06-12
---

# IStaffsAppService

CRUD contract for internal staff records with group membership assignment and lookup support.

## Public interface

Task<PagedResultDto<GetStaffForViewDto>> GetAll(GetAllStaffsInput input)
Task<GetStaffForViewDto> GetStaffForView(int id)
Task<GetStaffForEditOutput> GetStaffForEdit(EntityDto input)
Task CreateOrEdit(CreateOrEditStaffDto input)
Task Delete(EntityDto input)
Task<PagedResultDto<StaffGroupLookupTableDto>> GetAllGroupForLookupTable(GetAllForLookupTableInput input)
Task<List<StaffGroupLookupTableDto>> GetAllGroupForTableDropdown()

## External dependencies

- Abp.Core
