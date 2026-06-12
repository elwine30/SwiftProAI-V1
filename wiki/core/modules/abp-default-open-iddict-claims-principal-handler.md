---
title: "AbpDefaultOpenIddictClaimsPrincipalHandler"
type: service
language: csharp
layer: infrastructure
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/OpenIddict/Claims/AbpDefaultOpenIddictClaimsPrincipalHandler.cs"
updated: 2026-06-12
---

# AbpDefaultOpenIddictClaimsPrincipalHandler

Default handler in the OpenIddict claims pipeline that sets claim destinations (access token vs identity token) based on granted scopes and excludes security stamps from tokens.

## Public interface

virtual Task HandleAsync(AbpOpenIddictClaimsPrincipalHandlerContext context)

## External dependencies

- OpenIddict.Abstractions
- Abp.Dependency
