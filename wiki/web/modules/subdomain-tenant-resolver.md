---
title: "SubdomainTenantResolver"
type: class
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/shared/multi-tenancy/tenant-resolvers/subdomain-tenant-resolver.ts"
updated: 2026-06-12
---

# SubdomainTenantResolver

Resolves the current tenant name by extracting it from the URL subdomain during application bootstrap.

## Public interface

- `resolve(appBaseUrl): string`

## Dependencies

- [[subdomain-tenancy-name-finder]] — extracts the tenant name segment from the subdomain portion of a URL
