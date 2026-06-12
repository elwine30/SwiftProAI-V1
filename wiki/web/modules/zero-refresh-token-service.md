---
title: "ZeroRefreshTokenService"
type: service
language: typescript
layer: application
path: "SwiftProAI.Web/angular/src/account/auth/zero-refresh-token.service.ts"
updated: 2026-06-12
---

# ZeroRefreshTokenService

Implements the ABP RefreshTokenService contract to silently exchange a stored refresh token for a new access token and update SignalR query strings.

## Public interface

tryAuthWithRefreshToken(): Observable<boolean>

## External dependencies

- @angular/core
- abp-ng2-module
- rxjs

## Notes

After obtaining a new token it calls SignalRHelper.updateQueryString to keep the SignalR connection authenticated without a reconnect. Uses a Subject rather than BehaviorSubject, so late subscribers miss emissions.
