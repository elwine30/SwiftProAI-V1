---
title: "CaseCreditNote"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Registration/CaseCreditNote.cs"
updated: 2026-06-12
---

# CaseCreditNote

Entity representing a credit note issued to adjust a previously raised invoice downward on a case.

## Public interface

- Same itemised fee breakdown as `CaseInvoice` (service, mileage, photograph, toll, police, surveillance, etc.)
- `string CreditRefNo` / `string CreditRefNoPrefix` / `DateTime? InvoiceDate`
- `decimal TotalAmount` / `decimal? NetAmount` / `decimal? CreditAmount` / `decimal? DebitAmount`
- `string PaymentFlag` / `string PaymentMode` / `decimal? AmountPaid`
- `int? RegisterId` / `MainRegistration RegisterFk`
- `int? CompanyId` / `InsuranceCompany CompanyFk`
- `long? AdjusterId` / `User AdjusterFk`
- `int? CaseTypeId` / `CaseType CaseTypeFk`

## Dependencies

- [[main-registration]] parent registration FK
- [[insurance-company]] billed company FK
- [[case-type]] case type FK

## External dependencies

- Abp.Zero (FullAuditedEntity, IMustHaveTenant, IMayHaveOrganizationUnit)
