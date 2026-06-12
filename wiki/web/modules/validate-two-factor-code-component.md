---
title: "ValidateTwoFactorCodeComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/account/login/validate-two-factor-code.component.ts"
updated: 2026-06-12
---

# ValidateTwoFactorCodeComponent

Accepts the 2FA verification code and completes authentication via LoginService, with a countdown timer that expires the session after the configured seconds.

## Public interface

canActivate(): boolean
submit(): void
code: string
remainingSeconds: number

## Dependencies

- [[login-service]] completes the authentication flow after code validation
- [[recaptchav3-wrapper-service]] supplies reCAPTCHA token if required for 2FA verification

## Used by

- [[login-service]]

## External dependencies

- @angular/core
- @angular/router
- rxjs

## Notes

Uses rxjs timer with 1-second ticks and properly unsubscribes in ngOnDestroy. Reads twoFactorCodeExpireSeconds from appSession.application.
