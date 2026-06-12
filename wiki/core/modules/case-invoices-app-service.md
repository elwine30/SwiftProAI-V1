---
title: "CaseInvoicesAppService"
type: service
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application/Registration/CaseInvoicesAppService.cs"
updated: 2026-06-12
---

# CaseInvoicesAppService

Handles creation and retrieval of case invoices including auto-generated invoice reference numbers, charge rate lookup from configuration and company master data, and invoice preview data assembly.

## Public interface

Task<GetCaseInvoiceForPreviewDto> GetCaseInvoiceForPreview(int id)
Task<GetCaseInvoiceForViewDto> GetCaseInvoiceForView(int id)
Task<GetCaseInvoiceForEditOutput> GetCaseInvoiceForEdit(EntityDto input)
Task CreateOrEdit(CreateOrEditCaseInvoiceDto input)
Task Delete(EntityDto input)
Task<List<CaseInvoiceMainRegistrationLookupTableDto>> GetAllMainRegistrationForTableDropdown()
Task<List<CaseInvoiceUserLookupTableDto>> GetAllUserForTableDropdown()
Task<List<CaseInvoiceCaseTypeLookupTableDto>> GetAllCaseTypeForTableDropdown()

## Dependencies

- [[thinkn-insur-tech-app-service-base]] base class providing user, tenant and identity helpers

## External dependencies

- Abp.Zero

## Notes

Invoice reference numbers are padded sequential integers, prefix/length configurable per OU via DocumentSettings. Photograph charge defaults to appsettings but is overridden by the insurance company's PhotographCharge value. Throws UserFriendlyException if adjuster user no longer exists.
