---
title: "Branch"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Branches/Branch.cs"
updated: 2026-06-12
---

# Branch

Entity representing an organisational branch that is assigned to claim registrations and staff groups.

## Public interface

- `string Name`
- `string ShortName`
- `long? OrganizationUnitId`

## External dependencies

- Abp.Zero (FullAuditedEntity, IMustHaveTenant, IMayHaveOrganizationUnit)

## Notes

- Mandatory tenant (`IMustHaveTenant`) — branches are always tenant-scoped.
- Referenced by `MainRegistration`, `Group` and indirectly by `Staff`.
