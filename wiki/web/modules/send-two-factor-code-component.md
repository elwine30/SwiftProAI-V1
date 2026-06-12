---
title: "SendTwoFactorCodeComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/account/login/send-two-factor-code.component.ts"
updated: 2026-06-12
---

# SendTwoFactorCodeComponent

Lets the user choose a 2FA delivery channel and triggers the backend to send the code before navigating to the verification step.

## Public interface

canActivate(): boolean
submit(): void
selectedTwoFactorProvider: string

## Dependencies

- [[login-service]] used to send the 2FA code via the selected provider

## Used by

- [[login-service]]

## External dependencies

- @angular/core
- @angular/router
- rxjs

## Notes

Implements CanActivate directly on the component (not as a standalone guard) and calls canActivate() manually in ngOnInit as a self-guard pattern.
