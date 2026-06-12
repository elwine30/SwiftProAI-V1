---
title: "HomeController"
type: controller
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Host/Controllers/HomeController.cs"
updated: 2026-06-12
---

# HomeController

Handles the root URL by redirecting to the configured home page URL or falling back to the Ui/Index action in development.

## Public interface

- `[DisableAuditing] IActionResult Index()`

## External dependencies

- Abp.Auditing

## Notes

Auditing is disabled on Index to avoid noise. In development the redirect always goes to UiController.Index.
