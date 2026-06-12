---
title: "TenantBasedOpenIdConnectExternalLoginInfoProvider"
type: class
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Host/Startup/ExternalLoginInfoProviders/TenantBasedOpenIdConnectExternalLoginInfoProvider.cs"
updated: 2026-06-12
---

# TenantBasedOpenIdConnectExternalLoginInfoProvider

Resolves OIDC authority, client credentials, login URL and claim mappings from tenant or host settings for generic OpenID Connect provider support.

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

## Notes

Claim mappings are sourced from a separate AppSettings.ExternalLoginProvider.OpenIdConnectMappedClaims setting and merged into the provider extras.
