---
title: "LawFirm"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/LawFirms/LawFirm.cs"
updated: 2026-06-12
---

# LawFirm

Entity representing a legal firm that can be linked to a case via `CaseLawyer`.

## Public interface

- `string Name` / `string ShortName`
- `string Address`
- `bool IsActive`
- `string BusinessRegistrationNo`
- `long? OrganizationUnitId` / `long? AssignOUId`
- `bool AllowToViewAssignedCases`
- `int? ViewThirdPartyCaseRequestId` / `ViewThirdPartyCaseRequest? ViewThirdPartyCaseRequestFk`

## Dependencies

- [[view-third-party-case-request]] optional approval request FK

## External dependencies

- Abp.Zero (FullAuditedEntity, IMayHaveTenant, IMayHaveOrganizationUnit)

## Notes

- Mirrors the same third-party access-control pattern used by `Workshop` and `InsuranceCompany`.
