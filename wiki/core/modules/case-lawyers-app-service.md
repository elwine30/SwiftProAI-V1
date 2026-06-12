---
title: "CaseLawyersAppService"
type: service
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application/Registration/CaseLawyersAppService.cs"
updated: 2026-06-12
---

# CaseLawyersAppService

Manages the law firm assignment to a case, creating and maintaining the corresponding ViewThirdPartyCases record so the law firm's organisation can view the case.

## Public interface

Task<PagedResultDto<GetCaseLawyerForViewDto>> GetAll(GetAllCaseLawyersInput input)
Task<PagedResultDto<GetCaseLawyerForViewDto>> GetAllForView(GetAllCaseLawyersInput input)
Task<GetCaseLawyerForEditOutput> GetCaseLawyerForEdit(EntityDto input)
Task CreateOrEdit(CreateOrEditCaseLawyerDto input)
Task Delete(EntityDto input)
Task<PagedResultDto<CaseLawyerMainRegistrationLookupTableDto>> GetAllMainRegistrationForLookupTable(GetAllForLookupTableInput input)
Task<List<CaseLawyerLawFirmLookupTableDto>> GetAllLawFirmForTableDropdown()

## Dependencies

- [[thinkn-insur-tech-app-service-base]] base class providing user, tenant and identity helpers

## External dependencies

- Abp.Zero

## Notes

On Create, inserts a ViewThirdPartyCases record if the law firm has an AssignOUId (i.e. is onboarded). On Update, moves the ViewThirdPartyCases record to the new law firm's OU. On Delete, removes the ViewThirdPartyCases record. HearingDate defaults to DateTime.Now when invalid.
