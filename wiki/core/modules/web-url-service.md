---
title: "WebUrlService"
type: service
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Host/Url/WebUrlService.cs"
updated: 2026-06-12
---

# WebUrlService

Resolves the web site root and server root addresses from appsettings keys App:ClientRootAddress and App:ServerRootAddress.

## Public interface

- `override string WebSiteRootAddressFormatKey { get; }`
- `override string ServerRootAddressFormatKey { get; }`

## Used by

- [[startup]]
- [[angular-app-url-service]]
- [[payment-url-generator]]

## External dependencies

- Abp.Dependency
