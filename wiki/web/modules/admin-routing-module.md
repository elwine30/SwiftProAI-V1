---
title: "AdminRoutingModule"
type: module
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/app/admin/admin-routing.module.ts"
updated: 2026-06-12
---

# AdminRoutingModule

Defines all lazy-loaded child routes for the admin area, each decorated with an ABP permission string, and scrolls the window to the top on every NavigationEnd event.

## Used by

- [[admin-module]]

## External dependencies

- @angular/core
- @angular/router

## Notes

Route data carries `permission` strings (e.g. 'Pages.Administration.Users') that are consumed by AppRouteGuard. There are two duplicate route registrations for 'common/documentSettings' and 'registration/declarationQuestions' — likely copy-paste bugs. Default and wildcard routes both redirect to 'hostDashboard'.
