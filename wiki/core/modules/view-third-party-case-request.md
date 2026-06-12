---
title: "ViewThirdPartyCaseRequest"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Approval/ViewThirdPartyCaseRequest.cs"
updated: 2026-06-12
---

# ViewThirdPartyCaseRequest

Entity representing an approval request for one organisational unit to view cases assigned to another OU — supports the cross-organisation third-party case visibility workflow.

## Public interface

- `string Status` — pending / approved / rejected / cancelled
- `DateTime? ApprovedDate` / `int? ApprovedBy`
- `DateTime? RejectedDate` / `int? RejectedBy`
- `DateTime? CancelledDate` / `int? CancelledBy` / `string CancelRemark`
- `long? AssignByOU` / `long? AssignToOU`
- `string BusinessRegistrationNo` / `string CompanyName`

## External dependencies

- Abp.Zero (FullAuditedEntity, IMayHaveTenant)

## Notes

- Referenced as an optional FK on `InsuranceCompany`, `Workshop` and `LawFirm` — the requesting entity links to the approval record.
- `Status` is a free-text string — no enum enforcement; agreed values should be documented separately.
