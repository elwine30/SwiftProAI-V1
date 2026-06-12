---
title: "ICaseInvoicesAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Registration/ICaseInvoicesAppService.cs"
updated: 2026-06-12
---

# ICaseInvoicesAppService

Contract for managing and previewing financial invoices raised against insurance cases.

## Public interface

Task<GetCaseInvoiceForPreviewDto> GetCaseInvoiceForPreview(int id)
Task<GetCaseInvoiceForViewDto> GetCaseInvoiceForView(int id)
Task<GetCaseInvoiceForEditOutput> GetCaseInvoiceForEdit(EntityDto input)
Task CreateOrEdit(CreateOrEditCaseInvoiceDto input)
Task Delete(EntityDto input)
Task<List<CaseInvoiceMainRegistrationLookupTableDto>> GetAllMainRegistrationForTableDropdown()
Task<List<CaseInvoiceUserLookupTableDto>> GetAllUserForTableDropdown()
Task<List<CaseInvoiceCaseTypeLookupTableDto>> GetAllCaseTypeForTableDropdown()

## External dependencies

- Abp.Core
