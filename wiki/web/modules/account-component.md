---
title: "AccountComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/account/account.component.ts"
updated: 2026-06-12
---

# AccountComponent

Shell layout component for the account area, controlling logo skin, background image, and whether the tenant-switcher widget is shown.

## Public interface

showTenantChange(): boolean
isSelectEditionPage(): boolean
goToHome(): void
getBgUrl(): string
getLogoSkin(): string

## Dependencies

- [[login-service]] consulted to determine current tenant context for conditional rendering

## Used by

- [[account-module]]
- [[account-routing-module]]

## External dependencies

- @angular/core
- @angular/router
- lodash-es

## Notes

Reads abp.multiTenancy.isEnabled to gate the tenant-change UI. ViewEncapsulation.None is set so account-wide LESS styles bleed into child views.
