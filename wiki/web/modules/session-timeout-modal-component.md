---
title: "SessionTimeoutModalComponent"
type: component
language: typescript
layer: application
path: "SwiftProAI.Web/angular/src/app/shared/common/session-timeout/session-timeout-modal-component.ts"
updated: 2026-06-12
---

# SessionTimeoutModalComponent

Countdown modal displayed when the inactivity threshold is reached; auto-logs out or redirects to the lock screen when the countdown expires.

## Public interface

- `start(): void` — begin countdown and show modal
- `stop(): void` — cancel countdown and hide modal

## Dependencies

- [[app-auth-service]] performs logout or lock-screen redirect when the countdown expires

## Used by

- [[session-timeout-component]]
- [[app-auth-service]]

## External dependencies

- `@angular/core`
- `ngx-bootstrap/modal`
- `rxjs`

## Notes

Reads ShowLockScreenWhenTimedOut ABP setting to decide between lock-screen redirect and full logout. Stores userInfo in a cookie before redirecting to the lock screen.
