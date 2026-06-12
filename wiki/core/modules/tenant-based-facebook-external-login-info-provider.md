---
title: "TenantBasedFacebookExternalLoginInfoProvider"
type: class
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Host/Startup/ExternalLoginInfoProviders/TenantBasedFacebookExternalLoginInfoProvider.cs"
updated: 2026-06-12
---

# TenantBasedFacebookExternalLoginInfoProvider

Resolves Facebook OAuth app credentials from tenant or host settings, deserialising a JSON setting value into a typed provider info object.

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
