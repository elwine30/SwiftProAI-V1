---
title: "SessionLockScreenComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/account/login/session-lock-screen.component.ts"
updated: 2026-06-12
---

# SessionLockScreenComponent

Lock-screen overlay that re-authenticates the current user using a password, restoring the session without a full logout.

## Public interface

login(): void
getLastUserInfo(): void
userInfo: any

## Dependencies

- [[login-service]] re-authenticates the user's stored credentials against the lock screen password
- [[recaptchav3-wrapper-service]] supplies reCAPTCHA token if required for lock screen re-authentication

## Used by

- [[login-service]]

## External dependencies

- @angular/core

## Notes

Reads user info from the userInfo ABP cookie; navigates to root if that cookie is absent rather than throwing. Profile picture is fetched as a base64 JPEG string.
