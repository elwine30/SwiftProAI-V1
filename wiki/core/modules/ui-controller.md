---
title: "UiController"
type: controller
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Host/Controllers/UiController.cs"
updated: 2026-06-12
---

# UiController

Provides MVC Razor login/logout and homepage actions used by the OpenIddict server-side login page flow.

## Public interface

- `[DisableAuditing] async Task<IActionResult> Index()`
- `[HttpGet] async Task<IActionResult> Login(string returnUrl)`
- `[HttpPost] async Task<IActionResult> Login(LoginModel model, string returnUrl)`
- `async Task<ActionResult> Logout()`

## Dependencies

- [[login-model]] form model for the server-side Razor login page
- [[home-page-model]] view model carrying current login information for the Razor home page

## External dependencies

- Abp.Authorization
- Abp.Authorization.Users

## Notes

Used only when OpenIddict is enabled; the primary auth flow for Angular is JWT-based through the Application layer AccountAppService.
