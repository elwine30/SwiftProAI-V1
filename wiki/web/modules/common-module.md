---
title: "ThinknInsurTechCommonModule"
type: module
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/shared/common/common.module.ts"
updated: 2026-06-12
---

# ThinknInsurTechCommonModule

Root-level NgModule that registers the core session, URL and UI customisation providers for the whole application via forRoot().

## Public interface

- `static forRoot(): ModuleWithProviders<CommonModule>`

## Dependencies

- [[app-url-service]] registered as a root provider for tenant URL resolution
- [[app-ui-customization-service]] registered as a root provider for theme CSS class derivation
- [[app-session-service]] registered as a root provider for authenticated session state
- [[cookie-consent-service]] registered as a root provider for the cookie consent banner

## Used by

- [[app-component-base]]
- [[app-url-service]]

## External dependencies

- `@angular/core`
- `@angular/common`

## Notes

forRoot() returns CommonModule as the ngModule, not itself — a pattern that keeps the provider tree clean. The class name still carries the legacy 'ThinknInsurTech' codename.
