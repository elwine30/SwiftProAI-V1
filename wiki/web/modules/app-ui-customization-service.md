---
title: "AppUiCustomizationService"
type: service
language: typescript
layer: application
path: "SwiftProAI.Web/angular/src/shared/common/ui/app-ui-customization.service.ts"
updated: 2026-06-12
---

# AppUiCustomizationService

Derives and exposes CSS class names and style strings for the active Metronic theme based on the persisted UiCustomizationSettingsDto.

## Public interface

- `init(theme: UiCustomizationSettingsDto): void`
- `getAppModuleBodyClass(): string`
- `getAppModuleBodyStyle(): string`
- `getAccountModuleBodyClass(): string`
- `getLeftAsideClass(): string`
- `getLeftAsideSubMenuStyles(): string`
- `isSubmenuToggleDropdown(): boolean`
- `getTopBarMenuContainerClass(): string`
- `getIsMenuScrollable(): boolean`
- `getSideBarMenuItemClass(item, isMenuActive): string`

## Dependencies

- [[theme-asset-contributor-factory]] provides the active theme's body class via IThemeAssetContributor

## Used by

- [[app-component-base]]
- [[common-module]]

## External dependencies

- `@angular/core`
- `rtl-detect`

## Notes

Delegates body-class resolution to ThemeAssetContributorFactory; supports RTL via rtl-detect. Dark mode is driven by UiCustomizationSettingsDto.baseSettings.layout.darkMode and sets data-bs-theme attribute.
