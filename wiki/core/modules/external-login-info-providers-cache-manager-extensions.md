---
title: "ExternalLoginInfoProvidersCacheManagerExtensions"
type: class
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Host/Startup/ExternalLoginInfoProviders/ExternalLoginInfoProvidersCacheManagerExtensions.cs"
updated: 2026-06-12
---

# ExternalLoginInfoProvidersCacheManagerExtensions

Extension method that retrieves the strongly typed ABP cache instance used to store external OAuth provider configuration objects.

## Public interface

- `static ITypedCache<string, ExternalLoginProviderInfo> GetExternalLoginInfoProviderCache(this ICacheManager cacheManager)`

## Used by

- [[tenant-based-external-login-info-provider-base]]
- [[external-login-options-cache-manager]]

## External dependencies

- Abp.Runtime.Caching
- Abp.AspNetZeroCore.Web
