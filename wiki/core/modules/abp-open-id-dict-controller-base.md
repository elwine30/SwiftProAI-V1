---
title: "AbpOpenIdDictControllerBase"
type: class
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/OpenIddict/Controllers/AbpOpenIdDictControllerBase.cs"
updated: 2026-06-12
---

# AbpOpenIdDictControllerBase

Abstract base controller for OpenIddict endpoints providing shared access to OpenIddict managers, sign-in helpers, claims principal manager and pre-sign-in lockout checks.

## Public interface

protected virtual Task<OpenIddictRequest> GetOpenIddictServerRequestAsync(HttpContext httpContext)
protected virtual Task<IEnumerable<string>> GetResourcesAsync(ImmutableArray<string> scopes)
protected virtual Task<bool> HasFormValueAsync(string name)
protected virtual Task<bool> PreSignInCheckAsync(User user)

## Dependencies

- [[abp-open-iddict-claims-principal-manager]] invoked to enrich claims before token issuance

## Used by

- [[openiddict-token-controller]] extends this base for the token endpoint
- [[openiddict-authorize-controller]] extends this base for the authorisation endpoint
- [[openiddict-user-info-controller]] extends this base for the userinfo endpoint

## External dependencies

- OpenIddict.Abstractions
- Abp.AspNetCore.Mvc.Controllers
