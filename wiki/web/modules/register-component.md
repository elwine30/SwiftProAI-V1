---
title: "RegisterComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/account/register/register.component.ts"
updated: 2026-06-12
---

# RegisterComponent

Handles new user self-registration within a tenant context, optionally enforcing reCAPTCHA and auto-authenticating on success.

## Public interface

save(): void
model: RegisterModel
passwordComplexitySetting: PasswordComplexitySetting

## Dependencies

- [[login-service]] used to auto-authenticate the user after successful registration
- [[recaptchav3-wrapper-service]] enforces reCAPTCHA on the registration form

## Used by

- [[login-service]]

## External dependencies

- @angular/core
- @angular/router
- rxjs

## Notes

Guards against host-context registration by navigating to login if appSession.tenant is null.
