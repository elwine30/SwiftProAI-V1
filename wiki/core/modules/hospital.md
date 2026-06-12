---
title: "Hospital"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Registration/Hospital.cs"
updated: 2026-06-12
---

# Hospital

Reference entity for hospitals used in third-party injury tracking and insured-person admission records.

## Public interface

- `string Name`
- `string Address`
- `int? CountryLocationId` / `Location CountryLocationFk`
- `int? StateLocationId` / `Location StateLocationFk`

## Dependencies

- [[location]] country and state location FKs

## External dependencies

- Abp.Zero (FullAuditedEntity)

## Notes

- Not tenant-scoped (`IMayHaveTenant` not implemented) — hospitals are shared reference data.
- Referenced by `CaseInsuredPerson` and `CaseThirdPartyInfo` (up to three hospitals per third-party claimant).
