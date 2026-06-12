---
title: "AccountRoutingModule"
type: module
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/account/account-routing.module.ts"
updated: 2026-06-12
---

# AccountRoutingModule

Defines all lazy-loaded child routes under the account shell and toggles the body CSS class on each navigation event.

## Public interface

toggleBodyCssClass(url: string): void
setAccountModuleBodyClassInternal(): void

## Dependencies

- [[account-component]] shell layout component mounted at the account route root
- [[account-route-guard]] applied to account routes to redirect authenticated users

## Used by

- [[account-module]]

## External dependencies

- @angular/router

## Notes

Constructor subscribes to router.events to swap body CSS class via AppUiCustomizationService, preserving any swal2-toast-shown class already present.
