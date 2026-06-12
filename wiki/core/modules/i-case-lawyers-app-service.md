---
title: "ICaseLawyersAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Registration/ICaseLawyersAppService.cs"
updated: 2026-06-12
---

# ICaseLawyersAppService

Contract for attaching law firm lawyers to cases and providing lookup tables for legal firm selection.

## Public interface

Task<PagedResultDto<GetCaseLawyerForViewDto>> GetAll(GetAllCaseLawyersInput input)
Task<PagedResultDto<GetCaseLawyerForViewDto>> GetAllForView(GetAllCaseLawyersInput input)
Task<GetCaseLawyerForEditOutput> GetCaseLawyerForEdit(EntityDto input)
Task CreateOrEdit(CreateOrEditCaseLawyerDto input)
Task Delete(EntityDto input)
Task<PagedResultDto<CaseLawyerMainRegistrationLookupTableDto>> GetAllMainRegistrationForLookupTable(GetAllForLookupTableInput input)
Task<List<CaseLawyerLawFirmLookupTableDto>> GetAllLawFirmForTableDropdown()

## External dependencies

- Abp.Core
