---
title: "OCRController"
type: controller
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/Controllers/OCRController.cs"
updated: 2026-06-12
---

# OCRController

Accepts uploaded images (JPEG or PNG) and forwards them to ChatGPT-4o via IIntegrationService for OCR extraction of police reports, driving licence data and generic documents.

## Public interface

Task<JsonResult> UploadPoliceReport()
Task<JsonResult> UploadFrontDrivingLicense()
Task<JsonResult> UploadBackDrivingLicense()
Task<JsonResult> UploadDocument(string prompt)

## Dependencies

- [[thinkninsurtech-controller-base]] base class providing localisation and Identity helpers

## External dependencies

- SixLabors.ImageSharp

## Notes

UploadBackDrivingLicense and UploadDocument have commented-out live API calls with hardcoded mock responses active — these are clearly in-development and not production-ready. File size limit is 2 MB across all endpoints. GPT model is hardcoded to gpt-4o.
