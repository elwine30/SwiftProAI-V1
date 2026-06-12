---
title: "IFileOrgService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Integration/IFIleOrgService.cs"
updated: 2026-06-12
---

# IFileOrgService

Internal service contract for low-level file binary object operations including cache retrieval, metadata lookup and deletion.

## Public interface

Task<Guid?> GetBinaryObjectFromCache(string fileToken, int registerId, string folderField)
Task<Guid?> GetBinaryObjectFromCacheToId(string fileToken, int registerId, int folderId)
List<FileMetadataDto> GetMetadataByFolderIdAndCaseNo(int folderId, int caseNo)
FileMetadataDto GetMetadataByReference(Guid fileId)
Task<string> GetBinaryFileName(Guid? fileId)
Task DeleteFileByReference(Guid? referenceNo)
Task DeleteFileByMainEntityAndMainEntityID(string mainEntity, int mainEntityId)

## Notes

Filename in repo is IFIleOrgService.cs (capital I, lower l, capital O) — a typo in the filename.
