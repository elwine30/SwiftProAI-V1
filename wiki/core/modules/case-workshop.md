---
title: "CaseWorkshop"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Registration/CaseWorkshop.cs"
updated: 2026-06-12
---

# CaseWorkshop

Join entity linking a claim registration to its assigned repair workshop, with workshop-contact overrides at the case level.

## Public interface

- `string Email` / `string ContactNo` / `string ContactName`
- `int RegisterId` / `MainRegistration RegisterFk`
- `int? WorkshopId` / `Workshop WorkshopFk`

## Dependencies

- [[main-registration]] parent registration FK
- [[workshop]] referenced workshop

## External dependencies

- Abp.Zero (FullAuditedEntity, IMayHaveTenant, IMayHaveOrganizationUnit)
