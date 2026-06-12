---
title: "AngularAppUrlService"
type: service
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Host/Url/AngularAppUrlService.cs"
updated: 2026-06-12
---

# AngularAppUrlService

Provides Angular SPA route paths used in email links for activation, email change and password reset.

## Public interface

- `override string EmailActivationRoute { get; }`
- `override string EmailChangeRequestRoute { get; }`
- `override string PasswordResetRoute { get; }`

## Dependencies

- [[web-url-service]] resolves client and server root addresses from appsettings

## Used by

- [[startup]]

## External dependencies

- Abp.MultiTenancy
