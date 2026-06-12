---
title: "AuthorizeController"
type: controller
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/OpenIddict/Controllers/AuthorizeController.cs"
updated: 2026-06-12
---

# AuthorizeController

OpenIddict authorisation endpoint at connect/authorize that handles consent type logic (implicit, explicit, external, systematic) and issues authorisation codes or renders the consent form.

## Public interface

virtual Task<IActionResult> HandleAsync()
virtual Task<IActionResult> HandleCallbackAsync()

## Dependencies

- [[abp-open-id-dict-controller-base]] base class providing shared OpenIddict managers and helpers
- [[abp-open-iddict-claims-principal-manager]] invoked to run the claims pipeline before issuing codes

## External dependencies

- OpenIddict.Abstractions
- OpenIddict.Server.AspNetCore
