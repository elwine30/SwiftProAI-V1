---
title: "ScopeAssignment"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Registration/ScopeAssignment.cs"
updated: 2026-06-12
---

# ScopeAssignment

Reference entity describing the scope of work to be performed by an adjuster on a case assignment.

## Public interface

- `string Description` (required)
- `bool isActive`

## External dependencies

- Abp.Zero (FullAuditedEntity, IMayHaveTenant)

## Notes

- Note the non-standard casing `isActive` (lowercase `i`) — this is inconsistent with other entities that use `IsActive`.
- Referenced by `CaseAdjuster.ScopeAssignmentId`.
