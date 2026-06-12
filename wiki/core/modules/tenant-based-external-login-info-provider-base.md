---
title: "TenantBasedExternalLoginInfoProviderBase"
type: class
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Host/Startup/ExternalLoginInfoProviders/TenantBasedExternalLoginInfoProviderBase.cs"
updated: 2026-06-12
---

# TenantBasedExternalLoginInfoProviderBase

Abstract base that returns cached OAuth provider credentials, choosing tenant-specific settings when the current session belongs to a tenant, otherwise falling back to host-level settings.

## Public interface

- `abstract string Name { get; }`
- `virtual ExternalLoginProviderInfo GetExternalLoginInfo()`

## Dependencies

- [[external-login-info-providers-cache-manager-extensions]] retrieves the strongly typed ABP cache instance for OAuth provider configuration

## Used by

- [[tenant-based-facebook-external-login-info-provider]]
- [[tenant-based-twitter-external-login-info-provider]]
- [[tenant-based-google-external-login-info-provider]]
- [[tenant-based-microsoft-external-login-info-provider]]
- [[tenant-based-open-id-connect-external-login-info-provider]]
- [[tenant-based-ws-federation-external-login-info-provider]]

## External dependencies

- Abp.AspNetZeroCore.Web
- Abp.Runtime.Caching
- Abp.Runtime.Session

## Notes

Cache key is provider-name alone at host level or provider-name-tenantId at tenant level, enabling per-tenant OAuth app credentials.
