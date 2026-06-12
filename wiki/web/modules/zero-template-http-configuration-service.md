---
title: "ZeroTemplateHttpConfigurationService"
type: service
language: typescript
layer: application
path: "SwiftProAI.Web/angular/src/shared/service-proxies/zero-template-http-configuration.service.ts"
updated: 2026-06-12
---

# ZeroTemplateHttpConfigurationService

Overrides the ABP HTTP configuration to suppress 401-redirect behaviour while the user is on the login or session-locked pages.

## Public interface

- `handleUnAuthorizedRequest(messagePromise: any, targetUrl?: string): void`

## Used by

- [[service-proxy-module]]

## External dependencies

- `@angular/core`
- `@angular/router`
- `abp-ng2-module`

## Notes

Registered as the AbpHttpConfigurationService provider in ServiceProxyModule, replacing the default implementation. Prevents redirect loops when a 401 arrives during login. Relies on abp.appPath from the global abp namespace for the redirect target.
