---
title: "ICaseTypeAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Case/ICaseTypeAppService.cs"
updated: 2026-06-12
---

# ICaseTypeAppService

Contract for managing and listing case type definitions that categorise insurance claims.

## Public interface

Task<PagedResultDto<GetCaseTypeForViewDto>> GetAll(GetAllCaseTypeInput input)
Task<GetCaseTypeForViewDto> GetCaseTypeForView(int id)
Task<GetCaseTypeForEditOutput> GetCasetypeForEdit(EntityDto input)
Task<CaseTypeDto> GetCaseTypeDetailsbyId(int id)
Task<ListResultDto<CaseTypeDto>> GetAllCaseTypeDetails()
Task<int> CreateCaseType(CaseTypeDto input)

## Used by

- [[case-type-app-service]]

## External dependencies

- Abp.Core

## Notes

Missing UpdateCaseType and DeleteCaseType methods — the interface is create-and-read only.
