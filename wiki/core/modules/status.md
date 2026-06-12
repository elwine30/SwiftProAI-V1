---
title: "Status"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Case/Status.cs"
updated: 2026-06-12
---

# Status

Reference entity representing a case workflow status code with an optional close flag and type discriminator.

## Public interface

- `string Code`
- `string Description`
- `string Closeflag` — indicates whether this status marks a case as closed
- `string Type` — discriminates between case types or workflow stages

## External dependencies

- Abp.Zero (Entity<int>)

## Notes

- Unlike most entities, `Status` extends `Entity<int>` rather than `FullAuditedEntity` — there is no soft-delete or creation/modification tracking.
- `StatusManager` provides CRUD and lookup operations.
