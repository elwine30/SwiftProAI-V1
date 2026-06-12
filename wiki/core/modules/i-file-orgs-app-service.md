---
title: "IFileOrgsAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Common/IFileOrgsAppService.cs"
updated: 2026-06-12
---

# IFileOrgsAppService

Contract for uploading, renaming, deleting and retrieving files stored in the case file management system.

## Public interface

Task<FileMetadataDto> UploadFile(FileUploadInput input)
Task<String> RenameFileByReference(Guid referenceNo, string newFileName)
Task DeleteFileByReference(Guid referenceNo)
List<FileMetadataDto> GetMetadataByFolderAndCase(FileViewInputByFolderAndCase input)
Task<FilesViewDto> ViewFilesByFolderAndCase(FileViewInputByFolderAndCase input)
Task<FileViewDto> ViewFileByReference(FileViewInputByReference input)

## External dependencies

- Abp.Core

## Notes

GetMetadataByFolderAndCase is synchronous while ViewFilesByFolderAndCase is async — inconsistency in the same interface.
