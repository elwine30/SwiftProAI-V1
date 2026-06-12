---
title: "AppConsts"
type: class
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/shared/AppConsts.ts"
updated: 2026-06-12
---

# AppConsts

Holds all static global constants used across the application, including URLs, cookie names, grid defaults and version identifiers.

## Public interface

- `static remoteServiceBaseUrl: string`
- `static appBaseUrl: string`
- `static appBaseHref: string`
- `static recaptchaSiteKey: string`
- `static localeMappings: any[]`
- `static readonly tenancyNamePlaceHolderInUrl: string`
- `static readonly authorization.encrptedAuthTokenName: string`
- `static readonly localization.defaultLocalizationSourceName: string`
- `static readonly grid.defaultPageSize: number`
- `static readonly WebAppGuiVersion: string`

## Used by

- [[app-component-base]]
- [[app-url-service]]
- [[signalr-helper]]
- [[dynamic-resources-helper]]
- [[subdomain-tenancy-name-finder]]
- [[locale-mapping-service]]
- [[file-download-service]]
- [[localize-pipe]]
- [[friend-profile-picture-component]]

## Notes

Pure static class; all mutable statics (remoteServiceBaseUrl, appBaseUrl etc.) are written once by AppPreBootstrap at boot time from appconfig.json. The localization source name 'ThinknInsurTech' reveals the original product codename.
