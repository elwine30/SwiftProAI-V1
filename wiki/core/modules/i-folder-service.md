---
title: "IFolderService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Integration/IFolderService.cs"
updated: 2026-06-12
---

# IFolderService

Internal service contract for managing physical folder structures on disk mapped to case entities and fields.

## Public interface

List<FolderDto> GetAll(GetAllFoldersInput input)
List<FolderDto> GetAllByMainEntityAndId(string mainEntity, int mainEntityId)
Task<int> CreateByMainEntityAndField(string mainEntity, string field, int mainEntityId)
Task<Dictionary<string, Dictionary<string, int>>> GetAllInDictionary(int registerId)
Task MoveIntoFolderAsync(string sourcePath, string targetPath)
Task<string> GenerateDirectory(string folderField, string caseNo)
Task<string> GenerateDirectoryByFolderId(int folderId, string caseNo)
Task DeleteFolder(int folderId, int registerId)

## Notes

Does not inherit IApplicationService — used internally rather than exposed via ABP dynamic API.
