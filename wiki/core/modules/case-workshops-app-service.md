---
title: "CaseWorkshopsAppService"
type: service
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application/Registration/CaseWorkshopsAppService.cs"
updated: 2026-06-12
---

# CaseWorkshopsAppService

Manages workshop assignment to a case and mirrors the third-party visibility pattern used by law firm and insurer assignments.

## Public interface

Task<PagedResultDto<GetCaseWorkshopForViewDto>> GetAll(GetAllCaseWorkshopsInput input)
Task<GetCaseWorkshopForEditOutput> GetCaseWorkshopForEdit(EntityDto input)
Task CreateOrEdit(CreateOrEditCaseWorkshopDto input)
Task Delete(EntityDto input)
Task<List<CaseWorkshopMainRegistrationLookupTableDto>> GetAllMainRegistrationForTableDropdown()
Task<List<CaseWorkshopWorkshopLookupTableDto>> GetAllWorkshopForTableDropdown()

## Dependencies

- [[thinkn-insur-tech-app-service-base]] base class providing user, tenant and identity helpers

## External dependencies

- Abp.Zero

## Notes

Follows the same ViewThirdPartyCases creation pattern as CaseLawyersAppService.
