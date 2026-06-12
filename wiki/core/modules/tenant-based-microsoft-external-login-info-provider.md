---
title: "TenantBasedMicrosoftExternalLoginInfoProvider"
type: class
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Host/Startup/ExternalLoginInfoProviders/TenantBasedMicrosoftExternalLoginInfoProvider.cs"
updated: 2026-06-12
---

# TenantBasedMicrosoftExternalLoginInfoProvider

Resolves Microsoft OAuth2 client credentials from tenant or host settings for per-tenant Microsoft account login.

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
