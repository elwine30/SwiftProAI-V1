---
title: "PerRequestSessionCache"
type: service
language: csharp
layer: infrastructure
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/Session/PerRequestSessionCache.cs"
updated: 2026-06-12
---

# PerRequestSessionCache

Caches the current user session information in HttpContext.Items for the duration of a single HTTP request to avoid repeated database calls from multiple callers within the same request.

## Public interface

Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync()

## External dependencies

- Abp.Dependency
- Microsoft.AspNetCore.Http
