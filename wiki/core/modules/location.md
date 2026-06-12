---
title: "Location"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Common/Location.cs"
updated: 2026-06-12
---

# Location

Hierarchical reference entity for geographic locations (countries and states) used across incident and person addresses.

## Public interface

- `string ShortName`
- `string Name`
- `int ParentLocationId` — self-referencing FK to build country/state/city hierarchy

## External dependencies

- Abp.Zero (Entity)

## Notes

- Not tenant-scoped and not soft-deleted — shared global reference data.
- `ParentLocationId = 0` (or no parent) conventionally indicates a top-level country record.
- Referenced by `CaseInsuredPerson`, `CaseThirdPartyInfo`, `Hospital` and `CaseAdjuster`.
