---
title: "AppMenuItem"
type: class
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/app/shared/layout/nav/app-menu-item.ts"
updated: 2026-06-12
---

# AppMenuItem

Value object representing a single navigation menu entry with its permission name, icon, route and optional feature-dependency callback.

## Public interface

name, permissionName, icon, route, routeTemplates, items, external, requiresAuthentication, featureDependency, parameters
hasFeatureDependency(): boolean
featureDependencySatisfied(): boolean

## Used by

- [[app-navigation-service]]
- [[app-menu]]
- [[side-bar-menu-component]]
