---
title: "CaseAdjuster"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Registration/CaseAdjuster.cs"
updated: 2026-06-12
---

# CaseAdjuster

Entity recording an adjuster's assignment to a case, including the scope of the assignment and the state in which the assignment was made.

## Public interface

- `string Status` — adjuster assignment status
- `int? ScopeAssignmentId` / `ScopeAssignment ScopeAssignmentFk`
- `int RegisterId` / `MainRegistration RegisterFk`
- `int? StateLocationId` / `Location StateLocationFk`
- `long? EditorUserId` / `User EditorUserFk`
- `string ScopeAssignmentRemarks`

## Dependencies

- [[main-registration]] parent registration FK
- [[scope-assignment]] scope of work FK
- [[location]] state location FK

## External dependencies

- Abp.Zero (FullAuditedEntity<int>, IMustHaveTenant, IMayHaveOrganizationUnit)
