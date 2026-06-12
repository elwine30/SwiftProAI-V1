---
title: "AuditEntry"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Audit/AuditEntry.cs"
updated: 2026-06-12
---

# AuditEntry

Entity storing a single field-level change within an audit trail record — old value, new value and the field name.

## Public interface

- `string FieldName`
- `string OldValue`
- `string NewValue`
- `int? AuditTrailId` / `AuditTrail AuditTrailFk`

## Dependencies

- [[audit-trail]] parent audit trail FK

## External dependencies

- Abp.Zero (Entity, IMayHaveTenant)

## Notes

- Values are stored as strings regardless of the original property type — consumers must perform type conversion.
