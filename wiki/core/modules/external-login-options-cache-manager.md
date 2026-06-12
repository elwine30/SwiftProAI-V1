---
title: "ExternalLoginOptionsCacheManager"
type: class
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Host/Startup/ExternalLoginInfoProviders/ExternalLoginOptionsCacheManager.cs"
updated: 2026-06-12
---

# ExternalLoginOptionsCacheManager

Clears all cached social-login provider credentials for the current tenant or host, forcing fresh settings to be loaded on the next login attempt.

## Public interface

- `void ClearCache()`

## Dependencies

- [[external-login-info-providers-cache-manager-extensions]] retrieves the strongly typed ABP cache instance to perform the clear operation

## External dependencies

- Abp.Runtime.Caching
- Abp.Dependency
