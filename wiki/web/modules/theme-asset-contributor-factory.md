---
title: "ThemeAssetContributorFactory"
type: class
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/shared/helpers/ThemeAssetContributorFactory.ts"
updated: 2026-06-12
---

# ThemeAssetContributorFactory

Factory that returns the correct IThemeAssetContributor instance for the active theme, supporting all 13 Metronic theme variants.

## Public interface

- `static getCurrent(): IThemeAssetContributor`

## Used by

- [[app-ui-customization-service]]
- [[dynamic-resources-helper]]

## Notes

Reads the current theme name from ThemeHelper.getTheme() (which reads from abp.setting or localStorage). Returns null if the theme name is unrecognised — callers must null-check.
