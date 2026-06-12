---
title: "ChatController"
type: controller
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Host/Controllers/ChatController.cs"
updated: 2026-06-12
---

# ChatController

Serves uploaded chat attachment files by streaming binary objects from the object store to the browser.

## Public interface

- `async Task<ActionResult> GetUploadedObject(Guid fileId, string fileName, string contentType)`

## External dependencies

- Abp.AspNetCore.Mvc.Authorization

## Notes

Uses SetTenantId(null) scope so host-level binary objects can be retrieved regardless of the current tenant context.
