---
title: "CaseInsurer"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Registration/CaseInsurer.cs"
updated: 2026-06-12
---

# CaseInsurer

Entity capturing the insurer contact details associated with a specific claim registration.

## Public interface

- `string ReferenceNo` — insurer's own claim reference
- `string Name` / `string Contact` / `string Email`
- `int RegisterId` — parent registration (no FK navigation on this field)
- `int? CompanyId` / `InsuranceCompany CompanyFk`

## Dependencies

- [[insurance-company]] optional company FK
- [[main-registration]] via `RegisterId` (no navigation property defined)

## External dependencies

- Abp.Zero (FullAuditedEntity<int>, IMustHaveTenant, IMayHaveOrganizationUnit)
