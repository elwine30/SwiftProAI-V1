---
title: "ViewThirdPartyCases"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Organizations/ViewThirdPartyCases.cs"
updated: 2026-06-12
---

# ViewThirdPartyCases

Join entity granting a specific organisational unit read access to a case registration that belongs to another OU.

## Public interface

- `int RegisterId` / `MainRegistration MainRegistrationFk`
- `long? AssignedOUId`

## Dependencies

- [[main-registration]] referenced case registration

## External dependencies

- Abp.Zero (FullAuditedEntity, IMayHaveTenant)

## Notes

- Managed exclusively by `ViewThirdPartyCasesManager` — do not insert directly.
- Acts as the enforcement table: a case is visible to an OU only if a matching row exists here.
