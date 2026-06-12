---
title: "IViewThirdPartyCasesAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Organizations/IViewThirdPartyCasesAppService.cs"
updated: 2026-06-12
---

# IViewThirdPartyCasesAppService

Contract that allows third-party organisations to view and manage cases shared with them via the organisation portal.

## Public interface

Task<PagedResultDto<GetViewThirdPartyCasesForViewDto>> GetAll(GetAllViewThirdPartyCasesInput input)
Task<GetViewThirdPartyCasesForEditOutput> GetViewThirdPartyCasesForEdit(EntityDto input)
Task CreateOrEdit(CreateOrEditViewThirdPartyCaseDto input)
Task Delete(EntityDto input)

## Used by

- [[view-third-party-cases-app-service]]

## External dependencies

- Abp.Core
