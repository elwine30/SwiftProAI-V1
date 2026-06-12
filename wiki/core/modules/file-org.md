---
title: "FileOrg"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Common/FileOrg.cs"
updated: 2026-06-12
---

# FileOrg

Entity organising uploaded files against a claim registration and a named folder, using a GUID reference to the binary storage object.

## Public interface

- `Guid ReferenceNo` — BinaryObject storage key
- `string FileName` — includes file extension
- `int MainRegistrationId` / `MainRegistration MainRegistrationFk`
- `int? FolderId` / `Folder FolderFk`

## Dependencies

- [[main-registration]] parent registration FK
- [[folder]] organisational folder FK

## External dependencies

- Abp.Zero (FullAuditedEntity, IMayHaveTenant, IMayHaveOrganizationUnit)

## Notes

- `ReferenceNo` (GUID) is the key used to retrieve the actual file bytes from the `BinaryObject` / file-storage layer.
