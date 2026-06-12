---
title: "AuditTrail"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Audit/AuditTrail.cs"
updated: 2026-06-12
---

# AuditTrail

Concrete audit trail entity extending the abstract base; holds the collection of per-field change records for a single audited operation.

## Public interface

- `IEnumerable<AuditEntry> Changes` — field-level changes for this operation (from `AuditTrail.Extended.cs`)
- Inherits from `AuditTrailBase`: `string Operation`, `string TableName`, `string ChangedBy`, `long? OrganizationUnit`, `DateTime ChangedDate`

## Dependencies

- [[audit-entry]] one-to-many child records

## External dependencies

- Abp.Zero (Entity, IMayHaveTenant)

## Notes

- The class is split across `AuditTrail.cs` (extends `AuditTrailBase`) and `AuditTrail.Extended.cs` (adds the `Changes` navigation property) — the split allows the code generator to regenerate the base without overwriting custom additions.
- Populated by the custom `[Auditable]` / `[AuditedTrail]` attribute pipeline, not by ABP's built-in audit log.
