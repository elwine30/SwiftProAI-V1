---
title: "ICaseInsurersAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Registration/ICaseInsurersAppService.cs"
updated: 2026-06-12
---

# ICaseInsurersAppService

Contract for linking insurer entities to registered cases and retrieving insurer details per case.

## Public interface

Task<PagedResultDto<GetCaseInsurerForViewDto>> GetAll(GetAllCaseInsurersInput input)
Task<GetCaseInsurerForEditOutput> GetCaseInsurerForEdit(int Id)
Task CreateOrEdit(CreateOrEditCaseInsurerDto input)
Task<GetCaseInsurerForViewDto> GetCaseInsurerForView(EntityDto input)

## External dependencies

- Abp.Core

## Notes

Delete method is commented out — deletion of case-insurer links may be intentionally restricted.
