---
title: "UrlHelper"
type: class
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/shared/helpers/UrlHelper.ts"
updated: 2026-06-12
---

# UrlHelper

Provides static utility methods for parsing query parameters and detecting special URL conditions at boot time before the Angular router is active.

## Public interface

- `static readonly initialUrl: string`
- `static getQueryParameters(): any`
- `static getQueryParametersUsingParameters(search: string): any`
- `static getQueryParametersUsingHash(): any`
- `static getReturnUrl(): string`
- `static getSingleSignIn(): boolean`
- `static isInstallUrl(url): boolean`

## Used by

- [[query-string-tenant-resolver]]

## Notes

initialUrl is captured at module-load time (location.href), preserving pre-Angular-routing state. Used by AppPreBootstrap to detect SSO and return-URL redirects before the router initialises.
