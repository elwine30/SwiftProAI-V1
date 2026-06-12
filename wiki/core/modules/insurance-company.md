---
title: "InsuranceCompany"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Companies/InsuranceCompany.cs"
updated: 2026-06-12
---

# InsuranceCompany

Entity representing an insurance company (insurer) that is assigned to cases and invoices.

## Public interface

- `string Name` / `string ShortName`
- `decimal ClaimRate` — claim fee rate for this insurer
- `string Address`
- `string GstRegNo` / `string BusinessRegistrationNo`
- `bool IsActive`
- `decimal PhotographCharge`
- `int CaseTypeId` / `CaseType CaseTypeFk`
- `long? OrganizationUnitId` / `long? AssignOUId`
- `bool AllowToViewAssignedCases`
- `int? ViewThirdPartyCaseRequestId` / `ViewThirdPartyCaseRequest? ViewThirdPartyCaseRequestFk`

## Dependencies

- [[case-type]] foreign key `CaseTypeId`
- [[view-third-party-case-request]] optional approval request FK

## External dependencies

- Abp.Zero (FullAuditedEntity, IMustHaveTenant, IMayHaveOrganizationUnit)

## Notes

- `AllowToViewAssignedCases` and `ViewThirdPartyCaseRequestFk` implement the cross-OU third-party case visibility approval workflow.
- Validation constants for lengths and ranges are defined in `CompanyConsts` (Core.Shared).
