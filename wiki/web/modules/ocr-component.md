---
title: "OcrComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/main/ocr/ocr/ocr.component.ts"
updated: 2026-06-12
---

# OcrComponent

Standalone OCR page that uploads a police report image to an AI endpoint and maps the structured JSON response to a chatGPTOCROutput view model.

## Public interface

initUploader(): void — configures ng2-file-upload with bearer token
uploadLogo(): void — triggers upload with SweetAlert2 loading spinner
clearLogo(): void
checkFiles(file): void

## External dependencies

- @angular/core
- abp-ng2-module
- ng2-file-upload
- sweetalert2

## Notes

Uses regex match /\{[\s\S]*\}/ to extract JSON from ChatGPT response content — same brittle pattern as CreateOrEditCasePoliceReportComponent. AppConsts.remoteServiceBaseUrl is used to construct the upload URL, which is the correct pattern per project conventions.
