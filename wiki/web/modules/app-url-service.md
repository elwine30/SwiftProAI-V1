---
title: "AppUrlService"
type: service
language: typescript
layer: application
path: "SwiftProAI.Web/angular/src/shared/common/nav/app-url.service.ts"
updated: 2026-06-12
---

# AppUrlService

Resolves the application root URL for the current tenant, substituting the tenancy-name placeholder in the configured URL format.

## Public interface

- `get appRootUrl(): string`
- `getAppRootUrlOfTenant(tenancyName?: string): string`

## Dependencies

- [[app-consts]] provides the URL template with tenancy-name placeholder
- [[app-session-service]] provides the current tenant's tenancy name

## Used by

- [[app-component-base]]
- [[common-module]]

## External dependencies

- `@angular/core`

## Notes

Handles both subdomain-style ({TENANCY_NAME}.app.com) and path-style tenant URLs by replacing the placeholder. Returns URLs with a trailing slash.
