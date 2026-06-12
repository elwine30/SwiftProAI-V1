---
title: "ResetPasswordComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/account/password/reset-password.component.ts"
updated: 2026-06-12
---

# ResetPasswordComponent

Handles password reset from both email-link flows and forced-reset flows, resolving the correct tenant context before submitting and auto-logging in on success.

## Public interface

save(): void
parseTenantId(tenantIdAsStr?): number
model: ResetPasswordModel
passwordComplexitySetting: PasswordComplexitySetting

## Dependencies

- [[login-service]] used to auto-authenticate after a successful password reset
- [[recaptchav3-wrapper-service]] enforces reCAPTCHA on the reset form

## Used by

- [[login-service]]

## External dependencies

- @angular/core
- @angular/router
- rxjs

## Notes

Supports two query-string shapes: a compact c param (opaque token) and explicit userId+resetCode+tenantId params. Calls appSession.changeTenantIfNeeded to switch ABP tenant context before the reset call.
