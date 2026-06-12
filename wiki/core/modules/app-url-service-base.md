---
title: "AppUrlServiceBase"
type: service
language: csharp
layer: infrastructure
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/Url/AppUrlServiceBase.cs"
updated: 2026-06-12
---

# AppUrlServiceBase

Abstract base service that constructs email activation, email change and password reset URL formats by combining the site root address with route templates and query string placeholders.

## Public interface

abstract string EmailActivationRoute { get; }
abstract string EmailChangeRequestRoute { get; }
abstract string PasswordResetRoute { get; }
string CreateEmailActivationUrlFormat(int? tenantId)
string CreateEmailActivationUrlFormat(string tenancyName)
string CreateEmailChangeRequestUrlFormat(int? tenantId)
string CreateEmailChangeRequestUrlFormat(string tenancyName)
string CreatePasswordResetUrlFormat(int? tenantId)
string CreatePasswordResetUrlFormat(string tenancyName)

## External dependencies

- Abp.MultiTenancy
- Abp.Dependency
