---
title: "LawFirmsAppService"
type: service
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application/LawFirms/LawFirmsAppService.cs"
updated: 2026-06-12
---

# LawFirmsAppService

CRUD for law firm master data including third-party access request management (AllowToViewAssignedCases) mirroring the same pattern as CompanyAppService.

## Public interface

Task<PagedResultDto<GetLawFirmForViewDto>> GetAll(GetAllLawFirmsInput input)
Task<GetLawFirmForViewDto> GetLawFirmForView(int id)
Task<GetLawFirmForEditOutput> GetLawFirmForEdit(EntityDto input)
Task CreateOrEdit(CreateOrEditLawFirmDto input)
Task Delete(EntityDto input)

## Dependencies

- [[thinkn-insur-tech-app-service-base]] base class providing user, tenant and identity helpers

## External dependencies

- Abp.Zero

## Notes

Requires Pages_Administration_LawFirms permission. Manages ViewThirdPartyCaseRequest records using the same AllowToViewAssignedCases workflow as CompanyAppService.
