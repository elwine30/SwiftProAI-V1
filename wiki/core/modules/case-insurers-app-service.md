---
title: "CaseInsurersAppService"
type: service
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application/Registration/CaseInsurersAppService.cs"
updated: 2026-06-12
---

# CaseInsurersAppService

Manages the insurance company adjuster-contact record for a case, supporting both adjuster-owned and third-party read-only views.

## Public interface

Task<PagedResultDto<GetCaseInsurerForViewDto>> GetAll(GetAllCaseInsurersInput input)
Task<GetCaseInsurerForEditOutput> GetCaseInsurerForEdit(EntityDto input)
Task CreateOrEdit(CreateOrEditCaseInsurerDto input)
Task Delete(EntityDto input)
Task<List<CaseInsurerCompanyLookupTableDto>> GetAllCompanyForTableDropdown()

## Dependencies

- [[thinkn-insur-tech-app-service-base]] base class providing user, tenant and identity helpers

## External dependencies

- Abp.Zero

## Notes

Third-party view is gated behind ViewThirdPartyCases OU assignment check, disabling the HaveOrganizationUnit ABP filter.
