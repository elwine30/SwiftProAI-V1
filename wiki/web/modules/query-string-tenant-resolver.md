---
title: "QueryStringTenantResolver"
type: class
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/shared/multi-tenancy/tenant-resolvers/query-string-tenant-resolver.ts"
updated: 2026-06-12
---

# QueryStringTenantResolver

Resolves the current tenant name from the abp_tenancy_name query string parameter during application bootstrap.

## Public interface

- `resolve(): string`

## Dependencies

- [[url-helper]] — parses the current URL to extract query string parameters
