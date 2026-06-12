---
title: "SubdomainTenancyNameFinder"
type: class
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/shared/helpers/SubdomainTenancyNameFinder.ts"
updated: 2026-06-12
---

# SubdomainTenancyNameFinder

Extracts the tenant name from the current URL when subdomain-based multi-tenancy is configured.

## Public interface

- `urlHasTenancyNamePlaceholder(url: string): boolean`
- `getCurrentTenancyNameOrNull(rootAddress: string): string`

## Dependencies

- [[app-consts]] provides the tenancy-name placeholder constant used in URL matching
- [[formatted-string-value-extracter]] tokenises the URL template and extracts the dynamic tenant segment

## Used by

- [[subdomain-tenant-resolver]]
