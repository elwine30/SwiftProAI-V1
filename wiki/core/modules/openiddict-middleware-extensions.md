---
title: "OpenIddictMiddlewareExtensions"
type: class
language: csharp
layer: infrastructure
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/OpenIddict/OpenIddictMiddlewareExtensions.cs"
updated: 2026-06-12
---

# OpenIddictMiddlewareExtensions

Provides UseAbpOpenIddictValidation extension that injects an inline middleware to authenticate unauthenticated requests using the OpenIddict validation scheme.

## Public interface

static IApplicationBuilder UseAbpOpenIddictValidation(this IApplicationBuilder app, string schema)

## External dependencies

- OpenIddict.Validation.AspNetCore
