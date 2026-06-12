---
title: "CaseThirdPartyVehicleController"
type: controller
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Host/Controllers/CaseThirdPartyVehicleController.cs"
updated: 2026-06-12
---

# CaseThirdPartyVehicleController

Handles bulk vehicle-photo uploads and car-grant document uploads for third-party vehicles, with OCR triggered for car grant files.

## Public interface

- `List<FileUploadCacheOutput> TpvFileUpload()`
- `async Task<JsonResult> UploadDriverCarGrantFile()`
- `string[] GetTpvFileAllowedType()`
- `string[] GetDriverCarGrantFileAllowedTypes()`

## External dependencies

- Abp.IO.Extensions
- Abp.UI
- Abp.Web.Models

## Notes

TpvFileUpload accepts multiple files and returns a list of results, one per file. ValidateFile reads max size from OCR:MaxFileSize in appsettings rather than using the local constant.
