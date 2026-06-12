---
title: "Lookup"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Common/Lookup.cs"
updated: 2026-06-12
---

# Lookup

Generic key-value reference entity used as a configurable picklist throughout the domain — replaces hard-coded enums for extensible categorisation.

## Public interface

- `string Code` (required)
- `string Description` (required)
- `bool Active`
- `int Sequence` — display order within a group
- `string Group` (required) — discriminator identifying which picklist this row belongs to

## External dependencies

- Abp.Zero (Entity)

## Notes

- `IMayHaveTenant` is not implemented (`TenantId` is a plain nullable int) — cross-tenant shared lookups are possible.
- Used for expense types, claim statuses and other extensible categories across `CaseClaim`, `CaseExpense` and others.
