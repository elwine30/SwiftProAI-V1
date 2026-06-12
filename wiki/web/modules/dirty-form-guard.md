---
title: "DirtyFormGuard"
type: guard
language: typescript
layer: application
path: "SwiftProAI.Web/angular/src/app/shared/common/auth/dirty-form.guard.ts"
updated: 2026-06-12
---

# DirtyFormGuard

Prevents navigation away from a route if the active component reports unsaved form changes via the CanComponentDeactivate interface.

## Public interface

canDeactivate(component: CanComponentDeactivate): Observable<boolean> | Promise<boolean> | boolean

## External dependencies

- @angular/core
- @angular/router
- rxjs

## Notes

Exposes CanComponentDeactivate interface that components must implement. Registered with providedIn: 'root'.
