---
title: "SideBarMenuComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/shared/layout/nav/side-bar-menu.component.ts"
updated: 2026-06-12
---

# SideBarMenuComponent

Renders the Metronic aside/sidebar navigation menu and reinitialises Metronic JS components after each Angular route change.

## Public interface

@Input() iconMenu: boolean
@Input() menuClass: string
showMenuItem(menuItem): boolean
isMenuItemIsActive(item): boolean
getItemCssClasses(item, parentItem): string
getSubMenuItemCssClass(item, parentItem): string
reinitializeMenu(): void
scrollToCurrentMenuElement(): void

## Dependencies

- [[app-navigation-service]] provides the permission-filtered menu tree
- [[app-menu]] top-level menu container model
- [[app-menu-item]] individual menu entry value object

## External dependencies

- @angular/core
- @angular/router
- abp-ng2-module
- object-path
- rxjs

## Notes

Calls Metronic JS classes (MenuComponent, DrawerComponent, ToggleComponent, ScrollComponent) via @metronic path alias after navigation events to keep the DOM in sync with Angular routing.
