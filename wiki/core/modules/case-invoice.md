---
title: "CaseInvoice"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Registration/CaseInvoice.cs"
updated: 2026-06-12
---

# CaseInvoice

Entity representing an invoice raised against an insurer for adjuster services performed on a case; includes itemised amounts, SST, payment tracking and references to debit/credit adjustments.

## Public interface

- `decimal ServiceAmount` / `decimal ServiceSST` / `string ServiceRemark`
- `decimal MileageKM` / `decimal MileageUnitPrice` / `decimal MileageAmount`
- `decimal PhotographCharge` / `int PhotographQty` / `decimal PhotographTotalPrice`
- `decimal TollAmount` / `decimal PoliceAmount` / `decimal SurveillanceAmount`
- `decimal TotalAmount` / `decimal AmountExcludeSST` / `decimal AmountWithSST`
- `bool IncludeSST`
- `string InvoiceRefNo` / `string InvoiceRefNoPrefix` / `DateTime? InvoiceDate`
- `string InvoiceFlag` / `string InvoiceType` / `string PaymentFlag` / `string PaymentMode`
- `decimal? AmountPaid` / `string CheckNo` / `DateTime? DatePaid`
- `int RegisterId` / `MainRegistration RegisterFk`
- `int? CompanyId` / `InsuranceCompany CompanyFk`
- `long? ClaimExecutive` / `User ClaimExecutiveFk`
- `long AdjusterId` / `User AdjusterFk`
- `int CaseTypeId` / `CaseType CaseTypeFk`

## Dependencies

- [[main-registration]] parent registration FK
- [[insurance-company]] billed company FK
- [[case-type]] invoice case type FK

## External dependencies

- Abp.Zero (FullAuditedEntity, IMustHaveTenant, IMayHaveOrganizationUnit)

## Notes

- `TotalInTextForm` stores the total amount written out in words (for formal invoice documents).
- `InvoiceFlag` and `PaymentFlag` are string discriminators — values not enforced by enum.
- Mirrors `CaseDebitNote` and `CaseCreditNote` structurally; all three share the same fee line breakdown.
