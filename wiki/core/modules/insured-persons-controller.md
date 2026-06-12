---
title: "InsuredPersonsController"
type: controller
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Host/Controllers/InsuredPersonsController.cs"
updated: 2026-06-12
---

# InsuredPersonsController

Handles all document uploads for the insured driver (IC front/back, licence front/back, employment details, hospital details, car grant), invoking OCR for supported types.

## Public interface

- `async Task<JsonResult> UploadDriverICFrontFile()`
- `JsonResult UploadDriverICBackFile()`
- `async Task<JsonResult> UploadDriverLicenseFrontFile()`
- `async Task<JsonResult> UploadDriverLicenseBackFile()`
- `async Task<JsonResult> UploadDriverEmploymentDetailFile()`
- `async Task<JsonResult> UploadDriverHospitalDetailFile()`
- `async Task<JsonResult> UploadDriverCarGrantFile()`
- `string[] GetDriverICFrontFileAllowedTypes()`
- `string[] GetDriverICBackFileAllowedTypes()`

## External dependencies

- Abp.IO.Extensions
- Abp.UI

## Notes

HospitalDetail upload is intentionally not calling OCR (call commented out) — output is returned as null. ICBack also skips OCR. Seven redundant per-type constant sets all duplicate the same 5 MB limit.
