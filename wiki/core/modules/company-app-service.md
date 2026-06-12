---
title: "CompanyAppService"
type: service
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application/Companies/CompanyAppService.cs"
updated: 2026-06-12
---

# CompanyAppService

CRUD for insurance companies and manages the third-party case-view request lifecycle: submitting, cancelling and approving access grants between adjuster and insurance-company organisations.

## Public interface

Task<CompanyDto> GetCompanyDetailsbyId(int id)
Task<ListResultDto<CompanyDto>> GetAllCompanyDetails()
Task<PagedResultDto<GetCompanyForViewDto>> GetAll(GetAllCompaniesInput input)
Task<GetCompanyForViewDto> GetCompanyForView(int id)
Task<GetCompanyForEditOutput> GetCompanyForEdit(EntityDto input)
Task CreateOrEdit(CreateOrEditCompanyDto input)
Task Delete(EntityDto input)
Task<List<CompanyCaseTypeLookupTableDto>> GetAllCaseTypeForTableDropdown()

## Dependencies

- [[thinkn-insur-tech-app-service-base]] base class providing user, tenant and identity helpers

## External dependencies

- Abp.Zero

## Notes

On Create with AllowToViewAssignedCases, automatically creates a ViewThirdPartyCaseRequest with status 'Pending Approval'. On Update cancellation, deletes the associated ViewThirdPartyCases records and nullifies AssignOUId. Cancellation requires a CancelRemark.
