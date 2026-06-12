---
title: "MainRegistration"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Registration/MainRegistration.cs"
updated: 2026-06-12
---

# MainRegistration

The central aggregate root for a single insurance claim case, linking the adjuster, insurer, branch, status and all case sub-entities.

## Public interface

- `int CaseTypeId` / `CaseType CaseType`
- `long MemberId` — submitting member
- `int BranchId` / `Branch Branch`
- `int CompanyId` / `InsuranceCompany Company`
- `string VehicleNo`
- `DateTime AssignTime` / `DateTime? CompletionTime` / `DateTime? ExtendedCompletionDate`
- `string ModeOfAssignment`
- `long AdjusterMemberId` / `User AdjusterMemberFk`
- `long? EditorMemberId` / `User EditorMemberFk`
- `int? StatusId` / `Status Status`
- `string CaseNo` / `string FileRefNo` / `string Prefix`
- `ICollection<CaseInsuredPerson> InsuredPerson`
- `ICollection<InvoiceItem> InvoiceItems` / `DebitNoteItems` / `CreditNoteItems`

## Dependencies

- [[case-type]] foreign key `CaseTypeId`
- [[branch]] foreign key `BranchId`
- [[insurance-company]] foreign key `CompanyId`
- [[status]] foreign key `StatusId`
- [[case-insured-person]] one-to-many collection

## External dependencies

- Abp.Zero (FullAuditedEntity, IMustHaveTenant, IMayHaveOrganizationUnit)

## Notes

- Decorated with `[Auditable]` — all `[AuditedTrail]` properties are tracked in `AuditTrail` / `AuditEntry`.
- `long? OrganizationUnitId` supports multi-OU routing of cases.
- `InvoiceItems`, `DebitNoteItems` and `CreditNoteItems` are line-item collections navigated from `CaseInvoice`, `CaseDebitNote` and `CaseCreditNote` respectively.
