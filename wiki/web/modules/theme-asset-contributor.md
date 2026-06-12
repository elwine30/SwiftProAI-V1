---
title: "IThemeAssetContributor"
type: interface
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/app/shared/layout/themes/ThemeAssetContributor.ts"
updated: 2026-06-12
---

# IThemeAssetContributor

Contract that each per-theme asset contributor must implement to provide CSS/JS asset URLs, menu wrapper style, footer style and body attribute patches.

## Public interface

- `getAssetUrls(): string[]`
- `getMenuWrapperStyle(): string`
- `getFooterStyle(): string`
- `getBodyAttributes(): NameValuePair[]`
- `getAppModuleBodyClass(): string`
