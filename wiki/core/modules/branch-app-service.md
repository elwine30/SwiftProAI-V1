---
title: "BranchAppService"
type: service
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application/Branches/BranchAppService.cs"
updated: 2026-06-12
---

# BranchAppService

Simple CRUD for branch master data used as a geographic grouping for adjusters and cases.

## Public interface

Task<PagedResultDto<GetBranchForViewDto>> GetAll(GetAllBranchInput input)
Task<GetBranchForViewDto> GetBranchForView(int id)
Task<GetBranchForEditOutput> GetBranchForEdit(EntityDto input)
Task CreateOrEdit(CreateOrEditBranchDto input)
Task Delete(EntityDto input)

## Dependencies

- [[thinkn-insur-tech-app-service-base]] base class providing user, tenant and identity helpers

## External dependencies

- Abp.Zero

## Notes

Requires Pages_Branch permission. Minimal service with no special business rules.
