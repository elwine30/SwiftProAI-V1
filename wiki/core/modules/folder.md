---
title: "Folder"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Common/Folder.cs"
updated: 2026-06-12
---

# Folder

Entity representing a named document folder category for organising file uploads by entity type and field.

## Public interface

- `string MainEntity` — name of the domain entity this folder belongs to (e.g. "CaseInsuredPerson")
- `string Field` — the specific field or section within that entity
- `int? MainEntityId` — optional ID of the specific entity instance

## External dependencies

- Abp.Zero (FullAuditedEntity, IMayHaveOrganizationUnit)

## Notes

- Used by `FileOrg` to categorise uploaded files. `MainEntity` and `Field` together act as a logical path — equivalent to a virtual directory structure.
