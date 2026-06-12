---
title: "AppNavigationService"
type: service
language: typescript
layer: application
path: "SwiftProAI.Web/angular/src/app/shared/layout/nav/app-navigation.service.ts"
updated: 2026-06-12
---

# AppNavigationService

Builds the full application menu tree and controls per-item visibility based on ABP permissions and feature flags for the active session.

## Public interface

getMenu(): AppMenu — returns the full permission-filtered menu tree
showMenuItem(menuItem: AppMenuItem): boolean
checkChildMenuItemPermission(menuItem): boolean
getAllMenuItems(): AppMenuItem[]

## Dependencies

- [[app-menu]] container model holding the top-level menu and its children
- [[app-menu-item]] value object representing a single navigation menu entry

## Used by

- [[app-common-module]]
- [[side-bar-menu-component]]
- [[top-bar-menu-component]]

## External dependencies

- @angular/core
- abp-ng2-module

## Notes

Uses abp.multiTenancy.ignoreFeatureCheckForHostUsers to conditionally skip feature checks for host users. Menu items are defined statically here — any new route requires a manual addition.
