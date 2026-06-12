---
title: "ICachingAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Caching/ICachingAppService.cs"
updated: 2026-06-12
---

# ICachingAppService

Contract for listing, selectively clearing and bulk-clearing distributed cache entries from the admin interface.

## Public interface

ListResultDto<CacheDto> GetAllCaches()
Task ClearCache(EntityDto<string> input)
Task ClearAllCaches()
bool CanClearAllCaches()

## External dependencies

- Abp.Core

## Notes

CanClearAllCaches and GetAllCaches are synchronous — likely guarded operations checking host vs tenant context.
