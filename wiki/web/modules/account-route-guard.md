---
title: "AccountRouteGuard"
type: guard
language: typescript
layer: application
path: "SwiftProAI.Web/angular/src/account/auth/account-route-guard.ts"
updated: 2026-06-12
---

# AccountRouteGuard

Prevents authenticated users from accessing login and register routes by redirecting them to the most appropriate dashboard.

## Public interface

canActivate(route, state): boolean
selectBestRoute(): string

## Used by

- [[account-module]]
- [[account-routing-module]]

## External dependencies

- @angular/core
- @angular/router
- abp-ng2-module

## Notes

Uses the query param ss=true as an escape hatch to bypass the guard, supporting single-sign-in flows. Checks three ABP permission keys to pick the redirect destination.
