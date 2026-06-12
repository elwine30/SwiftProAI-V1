---
title: "TenantCustomizationController"
type: controller
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/Controllers/TenantCustomizationController.cs"
updated: 2026-06-12
---

# TenantCustomizationController

Manages per-tenant branding assets including light and dark logo variants (100 KB limit, 512x128 dimension validation) and custom CSS (1 MB limit), stored as binary objects.

## Public interface

Task<JsonResult> UploadLightLogo()
Task<JsonResult> UploadLightLogoMinimal()
Task<JsonResult> UploadDarkLogo()
Task<JsonResult> UploadDarkLogoMinimal()
Task<JsonResult> UploadCustomCss()
Task<ActionResult> GetLogo(int? tenantId)
Task<ActionResult> GetTenantLogo(string skin, int? tenantId, string extension)
Task<ActionResult> GetTenantLogoOrNull(string skin, int tenantId)
Task<ActionResult> GetCustomCss(int? tenantId)

## Dependencies

- [[thinkninsurtech-controller-base]] base class providing localisation and Identity helpers

## External dependencies

- Abp.AspNetZeroCore.Net
- Abp.MimeTypes
