---
title: "CaseDebitNote"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Registration/CaseDebitNote.cs"
updated: 2026-06-12
---

# CaseDebitNote

Entity representing a debit note issued to adjust a previously raised invoice upward on a case.

## Public interface

- Same itemised fee breakdown as `CaseInvoice` (service, mileage, photograph, toll, police, surveillance, etc.)
- `string DebitRefNo` / `string DebitRefNoPrefix` / `DateTime? DebitDate`
- `decimal TotalAmount` / `decimal? NetAmount` / `string TotalInTextForm`
- `string PaymentFlag` / `string PaymentMode` / `decimal? AmountPaid`
- `int RegisterId` / `MainRegistration RegisterFk`
- `int? CompanyId` / `InsuranceCompany CompanyFk`
- `long AdjusterId` / `User AdjusterFk`
- `int CaseTypeId` / `CaseType CaseTypeFk`

## Dependencies

- [[main-registration]] parent registration FK
- [[insurance-company]] billed company FK
- [[case-type]] case type FK

## External dependencies

- Abp.Zero (FullAuditedEntity, IMustHaveTenant, IMayHaveOrganizationUnit)

## Notes

- Structurally identical to `CaseInvoice` and `CaseCreditNote` — all three share the same fee line breakdown. This is intentional: debit and credit notes are independent documents, not adjustments stored on the invoice record.
