---
title: "TwoFactorCodeCacheExtensions"
type: class
language: csharp
layer: infrastructure
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/Authentication/TwoFactor/TwoFactorCodeCacheExtensions.cs"
updated: 2026-06-12
---

# TwoFactorCodeCacheExtensions

Extension method on ICacheManager that returns a strongly-typed cache for temporary two-factor verification codes.

## Public interface

static ITypedCache<string, TwoFactorCodeCacheItem> GetTwoFactorCodeCache(this ICacheManager cacheManager)

## Used by

- [[web-core-module]] registers cache configuration for two-factor code expiry
- [[token-auth-controller]] reads and clears two-factor codes during SendTwoFactorAuthCode and Authenticate flows

## External dependencies

- Abp.Runtime.Caching
