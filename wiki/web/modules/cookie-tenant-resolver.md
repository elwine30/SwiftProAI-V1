---
title: "CookieTenantResolver"
type: class
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/shared/multi-tenancy/tenant-resolvers/cookie-tenant-resolver.ts"
updated: 2026-06-12
---

# CookieTenantResolver

Resolves the current tenant name from the abp_tenancy_name browser cookie during application bootstrap.

## Public interface

- `resolve(): string`

## Notes

Relies on abp.utils.getCookieValue from the global ABP JS namespace. One of three tenant resolver strategies selected by AppPreBootstrap.
