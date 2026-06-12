---
title: "LinkedAccountService"
type: service
language: typescript
layer: application
path: "SwiftProAI.Web/angular/src/app/shared/layout/linked-account.service.ts"
updated: 2026-06-12
---

# LinkedAccountService

Switches the active session to a linked user account by obtaining a switch token from the API and then redirecting to the target tenant URL.

## Public interface

- `switchToAccount(userId: number, tenantId?: number): void`

## Dependencies

- [[app-auth-service]] clears the current session and performs the redirect to the target account

## Used by

- [[users-component]]
- [[users-module]]
- [[tenants-component]]

## External dependencies

- `@angular/core`

## Notes

Constructs the target URL via AppUrlService then delegates to AppAuthService.logout with that URL to clear the current session before redirecting.
