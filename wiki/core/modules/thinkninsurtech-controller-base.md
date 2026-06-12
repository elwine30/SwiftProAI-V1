---
title: "ThinknInsurTechControllerBase"
type: class
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/Controllers/ThinknInsurTechControllerBase.cs"
updated: 2026-06-12
---

# ThinknInsurTechControllerBase

Abstract base controller that sets the localisation source name and provides shared helpers for Identity error checking and multi-tenancy cookie management.

## Public interface

protected void CheckErrors(IdentityResult identityResult)
protected void SetTenantIdCookie(int? tenantId)

## Used by

- [[file-controller]] extends this base for file download endpoints
- [[ocr-controller]] extends this base for OCR upload endpoints
- [[profile-controller-base]] extends this base for profile picture endpoints
- [[stripe-controller-base]] extends this base for Stripe webhook endpoints
- [[excel-import-controller-base]] extends this base for Excel import endpoints
- [[tenant-customization-controller]] extends this base for branding asset endpoints

## External dependencies

- Abp.AspNetCore.Mvc.Controllers
- Microsoft.AspNetCore.Identity
