---
title: "JwtSecurityStampHandler"
type: service
language: csharp
layer: infrastructure
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/Authentication/JwtBearer/JwtSecurityStampHandler.cs"
updated: 2026-06-12
---

# JwtSecurityStampHandler

Validates and manages per-user security stamps for the one-concurrent-login feature, checking a distributed cache first before falling back to the database and caching the result.

## Public interface

Task<bool> Validate(ClaimsPrincipal claimsPrincipal)
Task SetSecurityStampCacheItem(int? tenantId, long userId, string securityStamp)
Task RemoveSecurityStampCacheItem(int? tenantId, long userId)

## Used by

- [[token-auth-controller]] calls SetSecurityStampCacheItem on login and RemoveSecurityStampCacheItem on logout
- [[thinkninsurtech-async-jwt-security-token-handler]] calls Validate on every token validation request

## External dependencies

- Abp.Runtime.Caching
- Abp.Domain.Uow
