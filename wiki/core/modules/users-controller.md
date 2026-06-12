---
title: "UsersController"
type: controller
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Host/Controllers/UsersController.cs"
updated: 2026-06-12
---

# UsersController

Receives an Excel file upload and enqueues a background job to import users from it.

## Public interface

- `override string ImportExcelPermission`
- `override async Task EnqueueExcelImportJobAsync(ImportFromExcelJobArgs args)`

## External dependencies

- Abp.BackgroundJobs
- Abp.AspNetCore.Mvc.Authorization

## Notes

Requires Pages_Administration_Users permission. Enqueues ImportUsersToExcelJob via ABP background job manager.
