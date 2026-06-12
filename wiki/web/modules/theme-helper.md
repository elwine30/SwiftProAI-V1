---
title: "ThemeHelper"
type: class
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/app/shared/layout/themes/ThemeHelper.ts"
updated: 2026-06-12
---

# ThemeHelper

Static utility that reads the active Metronic theme and its UI settings directly from the ABP settings namespace.

## Public interface

- `static getTheme(): string`
- `static darkMode(): boolean`
- `static getAsideSkin(): string`
- `static getAllowAsideMinimizing(): string`
- `static getDefaultMinimizedAside(): string`
- `static getHoverableAside(): string`
- `static getFixedAside(): string`
- `static getDesktopFixedHeader(): string`
- `static getMobileFixedHeader(): string`
- `static getDesktopFixedToolbar(): string`

## Notes

Entirely static class — all methods delegate to abp.setting.get(). Returns 'default' theme when no user is logged in.
