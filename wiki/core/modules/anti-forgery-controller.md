---
title: "AntiForgeryController"
type: controller
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Host/Controllers/AntiForgeryController.cs"
updated: 2026-06-12
---

# AntiForgeryController

Exposes a single action that sets the CSRF cookie and header so Angular clients can retrieve an anti-forgery token.

## Public interface

- `void GetToken()`

## External dependencies

- Microsoft.AspNetCore.Antiforgery

## Notes

IgnoreAntiforgeryTokenAttribute is applied globally in Startup, so this controller is effectively an opt-in token dispenser only.
