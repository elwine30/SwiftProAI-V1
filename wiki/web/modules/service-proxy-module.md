---
title: "ServiceProxyModule"
type: module
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/shared/service-proxies/service-proxy.module.ts"
updated: 2026-06-12
---

# ServiceProxyModule

NgModule that registers every NSwag-generated API service proxy as a provider and wires up the ABP HTTP interceptor, refresh-token service and custom HTTP configuration.

## Dependencies

- [[zero-template-http-configuration-service]] replaces the default ABP 401-handler to suppress redirect loops on the login page

## External dependencies

- `@angular/core`
- `@angular/common/http`
- `abp-ng2-module`

## Notes

Provides over 80 service proxies. The token-refresh service is provided as { provide: RefreshTokenService, useClass: ZeroRefreshTokenService } and HTTP interception is done via AbpHttpInterceptor (multi: true). Never edit generated proxy files — regenerate with npm run nswag.
