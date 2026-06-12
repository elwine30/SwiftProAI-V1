---
title: "CasePoliceReportsController"
type: controller
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Host/Controllers/CasePoliceReportsController.cs"
updated: 2026-06-12
---

# CasePoliceReportsController

Handles police report document uploads, stores the file in the temp cache, and immediately triggers OCR processing via IOCRPromptService.

## Public interface

- `async Task<JsonResult> UploadReportFileUploadFile()`
- `string[] GetReportFileUploadFileAllowedTypes()`

## External dependencies

- Abp.IO.Extensions
- Abp.UI

## Notes

caseId is read from the query string. OCR is invoked with UploadDocTypeEnum.PoliceReport and the base64-encoded file content. Returns FileOCROutput with both token and OCR result.
