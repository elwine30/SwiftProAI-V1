---
title: "DefaultLayoutComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/shared/layout/themes/default/default-layout.component.ts"
updated: 2026-06-12
---

# DefaultLayoutComponent

Root shell layout for the default Metronic theme, applying the aside-minimised attribute to the document body on init.

## Public interface

- `remoteServiceBaseUrl: string`
- `getMobileMenuSkin(): string`

## Dependencies

- [[themes-layout-base-component]] base class providing subscription and dark-mode helpers
- [[date-time-service]] used for date display in the layout shell

## External dependencies

- `@angular/core`
- `@angular/common`

## Notes

One of 13 structurally identical theme layouts (default, theme2–theme13). Applies data-kt-aside-minimize body attribute based on the persisted theme setting.
