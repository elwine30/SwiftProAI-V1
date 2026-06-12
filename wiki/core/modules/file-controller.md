---
title: "FileController"
type: controller
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/Controllers/FileController.cs"
updated: 2026-06-12
---

# FileController

Handles file downloads from three sources: temporary in-memory cache, binary object storage, and the filesystem-based FileOrg document store keyed by case number and folder metadata.

## Public interface

ActionResult DownloadTempFile(FileDto file)
Task<ActionResult> DownloadBinaryFile(Guid id, string contentType, string fileName)
Task<ActionResult> DownloadBinaryFileFromFileOrg(Guid id, string contentType, string fileName)

## Dependencies

- [[thinkninsurtech-controller-base]] base class providing localisation and Identity helpers
- [[app-configuration-accessor]] reads Folder.root configuration for FileOrg path construction

## External dependencies

- Abp.MimeTypes
- Abp.Auditing

## Notes

DownloadBinaryFileFromFileOrg builds a filesystem path from Folder.root config, tenant ID, case number, entity name and field name. Console.WriteLine(rootFolder) is present — likely a debug leftover.
