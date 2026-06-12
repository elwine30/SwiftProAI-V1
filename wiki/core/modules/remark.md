---
title: "Remark"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Remarks/Remark.cs"
updated: 2026-06-12
---

# Remark

Entity recording a free-text adjuster remark or note attached to a case registration.

## Public interface

- `int RegisterId`
- `string Description`

## External dependencies

- Abp.Zero (CreationAuditedEntity<int>)

## Notes

- Uses `CreationAuditedEntity` rather than `FullAuditedEntity` — remarks are immutable once created (no update/delete audit columns).
- Not tenant-scoped at the entity level; tenant scoping is applied at query time via ABP data filters.
