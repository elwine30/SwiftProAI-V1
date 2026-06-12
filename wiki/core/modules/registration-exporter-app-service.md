---
title: "RegistrationExporterAppService"
type: service
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application/Registration/RegistrationExporterAppService.cs"
updated: 2026-06-12
---

# RegistrationExporterAppService

Generates a fully populated .docx investigation report from a Word template by substituting named text placeholders and embedding IC and licence images.

## Public interface

Task<FileDto> PostExportRegistration(int registerId)

## Dependencies

- [[thinkn-insur-tech-app-service-base]] base class providing user, tenant and identity helpers

## External dependencies

- DocumentFormat.OpenXml
- Abp.AspNetZeroCore

## Notes

Uses DocumentFormat.OpenXml (Open XML SDK) to manipulate the template in-memory. Image bytes are fetched from FileOrgService by GUID reference. Template file is resolved via reflection from the assembly directory. No permission attribute — class is not decorated with AbpAuthorize.
