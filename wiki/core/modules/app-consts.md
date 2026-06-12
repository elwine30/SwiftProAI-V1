---
title: "AppConsts"
type: class
language: csharp
layer: shared
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/AppConsts.cs"
updated: 2026-06-12
---

# AppConsts

Central repository of application-wide constants including pagination defaults, JWT token keys, pass phrases and theme name strings.

## Public interface

const int DefaultPageSize = 10
const int MaxPageSize = 1000
const string DefaultPassPhrase
const string TokenValidityKey
const string RefreshTokenValidityKey
static TimeSpan AccessTokenExpiration (1 day)
static TimeSpan RefreshTokenExpiration (365 days)

## Notes

DefaultPassPhrase is a hardcoded string — should be rotated per environment. RefreshTokenExpiration is 365 days which is unusually long.
