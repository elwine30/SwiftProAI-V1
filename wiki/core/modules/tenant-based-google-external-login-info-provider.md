---
title: "TenantBasedGoogleExternalLoginInfoProvider"
type: class
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Host/Startup/ExternalLoginInfoProviders/TenantBasedGoogleExternalLoginInfoProvider.cs"
updated: 2026-06-12
---

# TenantBasedGoogleExternalLoginInfoProvider

Resolves Google OAuth2 client credentials and user-info endpoint from tenant or host settings, including the custom user-info URL in the provider extras dictionary.

## Public interface

- `override string Name { get; }`
- `override ExternalLoginProviderInfo GetExternalLoginInfo()`

## Dependencies

- [[tenant-based-external-login-info-provider-base]] abstract base providing cached credential resolution with tenant fallback logic

## Used by

- [[thinkn-insur-tech-web-host-module]]

## External dependencies

- Abp.AspNetZeroCore.Web
- Abp.Configuration
- Abp.Dependency
