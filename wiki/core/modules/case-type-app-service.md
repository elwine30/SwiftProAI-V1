---
title: "CaseTypeAppService"
type: service
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application/Case/CaseTypeAppService.cs"
updated: 2026-06-12
---

# CaseTypeAppService

CRUD management of insurance case types with active/inactive filtering and lookup table support for dropdowns.

## Public interface

Task<PagedResultDto<GetCaseTypeForViewDto>> GetAll(GetAllCaseTypeInput input)
Task<GetCaseTypeForViewDto> GetCaseTypeForView(int id)
Task<GetCaseTypeForEditOutput> GetCasetypeForEdit(EntityDto input)
Task CreateOrEdit(CreateOrEditCaseTypeDto input)
Task Delete(EntityDto input)
Task<CaseTypeDto> GetCaseTypeDetailsbyId(int id)
Task<ListResultDto<CaseTypeDto>> GetAllCaseTypeDetails()
Task<int> CreateCaseType(CaseTypeDto input)

## Dependencies

- [[thinkn-insur-tech-app-service-base]] base class providing user, tenant and identity helpers

## External dependencies

- Abp.Zero

## Notes

Dual API pattern: newer GetAll/CreateOrEdit methods alongside older GetAllCaseTypeDetails/CreateCaseType methods that delegate to ICaseTypeManager domain service.
