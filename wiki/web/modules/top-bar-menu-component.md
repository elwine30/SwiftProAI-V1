---
title: "TopBarMenuComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/shared/layout/nav/top-bar-menu.component.ts"
updated: 2026-06-12
---

# TopBarMenuComponent

Renders the horizontal top-bar navigation menu with active-route highlighting and Metronic component reinitialisation on route changes.

## Public interface

@Input() menuClass: string
showMenuItem(menuItem): boolean
isMenuItemIsActive(item): boolean
reinitializeMenu(): void
getItemCssClasses(item, parentItem, depth): string
getSubmenuCssClasses(item, parentItem, depth): string
isMobileDevice(): any

## Dependencies

- [[app-navigation-service]] provides the permission-filtered menu tree
- [[app-menu]] top-level menu container model

## External dependencies

- @angular/core
- @angular/router
- abp-ng2-module
- object-path
- rxjs

## Notes

Reads menu wrapper style from ThemeAssetContributorFactory to apply theme-specific CSS at runtime.
