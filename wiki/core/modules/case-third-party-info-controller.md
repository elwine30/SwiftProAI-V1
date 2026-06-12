---
title: "CaseThirdPartyInfoController"
type: controller
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Host/Controllers/CaseThirdPartyInfoController.cs"
updated: 2026-06-12
---

# CaseThirdPartyInfoController

Handles multi-type document uploads for third-party personal information (IC, licence, employment), routing each to the appropriate OCR document type.

## Public interface

- `async Task<FileUploadCacheOutput> TpiFileUpload()`
- `string[] GetTpiFileAllowedType()`

## External dependencies

- Abp.IO.Extensions
- Abp.UI
- Abp.Web.Models

## Notes

fileUploadType query parameter drives the UploadDocTypeEnum switch. HospitalDetail is commented out. OCR is skipped when UploadType is None.
