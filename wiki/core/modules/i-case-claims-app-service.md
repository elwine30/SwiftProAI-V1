---
title: "ICaseClaimsAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Registration/ICaseClaimsAppService.cs"
updated: 2026-06-12
---

# ICaseClaimsAppService

CRUD contract for claim records attached to a main registration case.

## Public interface

Task<GetCaseClaimForEditOutput> GetCaseClaimForEdit(EntityDto input)
Task CreateOrEdit(CreateOrEditCaseClaimDto input)
Task<PagedResultDto<CreateOrEditCaseClaimDto>> GetAll(CaseClaimMainRegistrationInput input)

## External dependencies

- Abp.Core
