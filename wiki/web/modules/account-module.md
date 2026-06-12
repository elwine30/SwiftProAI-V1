---
title: "AccountModule"
type: module
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/account/account.module.ts"
updated: 2026-06-12
---

# AccountModule

Root feature module for all unauthenticated routes, wiring providers for login, registration, payment and reCAPTCHA.

## Public interface

providers: [LoginService, TenantRegistrationHelperService, PaymentHelperService, AccountRouteGuard, ReCaptchaV3WrapperService]
RECAPTCHA_V3_SITE_KEY bound to AppConsts.recaptchaSiteKey

## Dependencies

- [[account-routing-module]] defines child routes for the account shell
- [[account-component]] shell layout component for account area
- [[account-route-guard]] prevents authenticated users accessing account routes
- [[language-switch-component]] language dropdown for account pages
- [[login-service]] orchestrates credential and social-provider authentication
- [[tenant-registration-helper-service]] transient state holder for registration flow
- [[payment-helper-service]] translates gateway enum to route path strings
- [[recaptchav3-wrapper-service]] abstracts reCAPTCHA v3 usage across account forms
- [[account-shared-module]] exports tenant-change component pair

## Used by

No entries.

## External dependencies

- @angular/common
- @angular/forms
- @angular/common/http
- ng-recaptcha
- ngx-bootstrap/modal
- angular-oauth2-oidc
- primeng/password

## Notes

Calls abp.localization.currentLanguage via getRecaptchaLanguage factory function passed to RECAPTCHA_V3_SITE_KEY provider.
