---
title: "FileOcrController"
type: controller
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Host/Controllers/FileOcrController.cs"
updated: 2026-06-12
---

# FileOcrController

General-purpose OCR upload endpoint supporting single and multi-file uploads with PDF-to-image conversion before sending to the OCR service.

## Public interface

- `string[] GetFileAllowedTypes()`
- `async Task<JsonResult> UploadFile()`
- `async Task<JsonResult> UploadFiles()`

## External dependencies

- Abp.IO.Extensions
- Abp.UI
- PdfiumViewer
- GraphQL

## Notes

UploadFile converts PDF pages to PNG via PdfiumViewer at 300 DPI before OCR. UploadFiles bulk-processes files but has the PDF conversion and OCR calls commented out. HospitalDetail type skips OCR. File names longer than 23 chars are truncated with ellipsis in the response.
