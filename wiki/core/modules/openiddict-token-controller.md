---
title: "TokenController"
type: controller
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/OpenIddict/Controllers/TokenController.cs"
updated: 2026-06-12
---

# TokenController

OpenIddict token endpoint controller at connect/token that dispatches password and authorisation code grant flows and extracts tenant ID from claims.

## Public interface

Task<IActionResult> HandleAsync()
protected virtual Task<IActionResult> HandlePasswordAsync(OpenIddictRequest request)
protected virtual Task<IActionResult> HandleAuthorizationCodeAsync(OpenIddictRequest request)
protected virtual Task<IActionResult> SetSuccessResultAsync(OpenIddictRequest request, User user)
protected virtual Task<IEnumerable<string>> GetResourcesAsync(ImmutableArray<string> scopes)

## Dependencies

- [[abp-open-id-dict-controller-base]] base class providing shared OpenIddict managers and helpers
- [[abp-open-iddict-claims-principal-manager]] invoked to run the claims pipeline before signing in

## External dependencies

- OpenIddict.Abstractions
- OpenIddict.Server.AspNetCore

## Notes

Implemented as a partial class split across three files (TokenController.cs, TokenController.Password.cs, TokenController.AuthorizationCode.cs). HandlePasswordAsync contains a TODO comment about tenant switching. IocManager.Instance is used directly inside partial methods.
