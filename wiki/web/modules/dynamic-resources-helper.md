---
title: "DynamicResourcesHelper"
type: class
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/shared/helpers/DynamicResourcesHelper.ts"
updated: 2026-06-12
---

# DynamicResourcesHelper

Dynamically loads the correct Metronic theme CSS bundles and PrimeNG styles at boot time, applying RTL and dark-mode variants as needed.

## Public interface

- `static loadResources(callback: () => void): void`
- `static loadStyles(): Promise<any>`
- `static getAdditionalThemeAssets(): string[]`
- `static setBodyAttributes(): void`

## Dependencies

- [[app-consts]] provides the remote service base URL for constructing asset paths
- [[style-loader-service]] injects CSS link elements into the document head
- [[theme-asset-contributor-factory]] provides per-theme asset URL lists and body attributes

## External dependencies

- `rtl-detect`

## Notes

Fires abp.event.trigger('app.dynamic-styles-loaded') after CSS loads complete. Sets document dir=rtl and data-bs-theme on the html element. Called from AppPreBootstrap before Angular bootstraps.
