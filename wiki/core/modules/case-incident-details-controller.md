---
title: "CaseIncidentDetailsController"
type: controller
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Host/Controllers/CaseIncidentDetailsController.cs"
updated: 2026-06-12
---

# CaseIncidentDetailsController

Handles file uploads for incident circumstance documents, validating file type and size before storing to the temp file cache.

## Public interface

- `FileUploadCacheOutput UploadCircumstancesFileUploadFile()`
- `string[] GetCircumstancesFileUploadFileAllowedTypes()`

## External dependencies

- Abp.IO.Extensions
- Abp.UI
- Abp.Web.Models

## Notes

5 MB hard limit. Allowed file types are loaded from FileUpload:AllowedFileTypes in appsettings. The hard-coded array in the source is commented out, meaning types are config-driven only.
