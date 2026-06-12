---
title: "Group"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Common/Group.cs"
updated: 2026-06-12
---

# Group

Entity representing a named organisational group that staff are assigned to, linked to a specific branch.

## Public interface

- `string Name` (required)
- `int GroupType` — numeric discriminator for the group category
- `bool IsActive`
- `int BranchId` / `Branch BranchFk`

## Dependencies

- [[branch]] parent branch FK

## External dependencies

- Abp.Zero (FullAuditedEntity, IMustHaveTenant, IMayHaveOrganizationUnit)
