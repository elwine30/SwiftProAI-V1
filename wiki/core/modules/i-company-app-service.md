---
title: "ICompanyAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Companies/ICompanyAppService.cs"
updated: 2026-06-12
---

# ICompanyAppService

CRUD contract for insurance company records, with lookup helpers for case type association.

## Public interface

Task<CompanyDto> GetCompanyDetailsbyId(int id)
Task<ListResultDto<CompanyDto>> GetAllCompanyDetails()
Task<PagedResultDto<GetCompanyForViewDto>> GetAll(GetAllCompaniesInput input)
Task<GetCompanyForViewDto> GetCompanyForView(int id)
Task<GetCompanyForEditOutput> GetCompanyForEdit(EntityDto input)
Task CreateOrEdit(CreateOrEditCompanyDto input)
Task Delete(EntityDto input)
Task<List<CompanyCaseTypeLookupTableDto>> GetAllCaseTypeForTableDropdown()

## Used by

- [[company-app-service]]

## External dependencies

- Abp.Core
