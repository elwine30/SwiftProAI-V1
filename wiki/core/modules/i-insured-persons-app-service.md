---
title: "IInsuredPersonsAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Registration/IInsuredPersonsAppService.cs"
updated: 2026-06-12
---

# IInsuredPersonsAppService

Contract for managing insured person (driver and owner) records including identity document and licence file management.

## Public interface

Task<GetInsuredPersonForEditOutput> GetInsuredPersonForEdit(EntityDto input, bool isOwner)
Task<bool?> CreateOrEdit(CreateOrEditInsuredPersonDto input)
Task Delete(EntityDto input)
Task RemoveDriverICFrontFile(EntityDto input)
Task RemoveDriverICBackFile(EntityDto input)
Task RemoveDriverLicenseFrontFile(EntityDto input)
Task RemoveDriverLicenseBackFile(EntityDto input)
Task RemoveDriverHospitalDetailFile(EntityDto input)
Task<GetInsuredPersonForViewDto> GetInsuredPersonForView(EntityDto input, bool isOwner)

## External dependencies

- Abp.Core

## Notes

isOwner flag distinguishes owner vs driver — a single service handles both person roles.
