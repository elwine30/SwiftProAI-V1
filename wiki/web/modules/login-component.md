---
title: "LoginComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/account/login/login.component.ts"
updated: 2026-06-12
---

# LoginComponent

Renders the login form and delegates credential submission and external OAuth callbacks to LoginService, bootstrapping the correct tenant on load.

## Public interface

login(): void
externalLogin(provider: ExternalLoginProvider): void
handleExternalLoginCallbacks(): void
get multiTenancySideIsTeanant(): boolean
get isTenantSelfRegistrationAllowed(): boolean
get isSelfRegistrationAllowed(): boolean

## Dependencies

- [[login-service]] handles credential submission and external OAuth flow
- [[recaptchav3-wrapper-service]] manages reCAPTCHA token retrieval for the login form

## External dependencies

- @angular/core
- @angular/router
- abp-ng2-module

## Notes

Constructor resolves the Default tenant and sets the ABP tenant-id cookie unless the URL contains an /internal segment, which bypasses tenant resolution for internal logins. Calls KTApp (Metronic global) indirectly via PasswordMeterComponent.bootstrap().
