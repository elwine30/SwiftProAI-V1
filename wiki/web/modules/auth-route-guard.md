---
title: "AppRouteGuard"
type: guard
language: typescript
layer: application
path: "SwiftProAI.Web/angular/src/app/shared/common/auth/auth-route-guard.ts"
updated: 2026-06-12
---

# AppRouteGuard

Protects all authenticated app routes by checking for an active session and then validating ABP permission names declared in route data, with automatic refresh-token retry.

## Public interface

canActivate(route, state): Observable<boolean>
canActivateChild(route, state): Observable<boolean>
canLoad(route): Observable<boolean>
canActivateInternal(data, state): Observable<boolean>
selectBestRoute(): string — returns role-appropriate landing route

## Dependencies

- [[app-session-service]] provides the current session state and triggers refresh-token flow

## Used by

- [[app-common-module]]

## External dependencies

- @angular/core
- @angular/router
- abp-ng2-module
- rxjs

## Notes

Implements CanActivate, CanActivateChild and CanLoad; uses Subject to bridge the async refresh-token observable back to the guard return type. selectBestRoute() encodes business-level role priority (host dashboard > tenant dashboard > tenants > users).
