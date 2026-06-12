---
title: "ReCaptchaV3WrapperService"
type: service
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/account/shared/recaptchav3-wrapper.service.ts"
updated: 2026-06-12
---

# ReCaptchaV3WrapperService

Abstracts reCAPTCHA v3 usage by reading ABP settings to determine whether CAPTCHA is required and managing badge visibility for login and registration pages.

## Public interface

getService(): ReCaptchaV3Service
useCaptchaOnLogin(): boolean
setCaptchaVisibilityOnLogin(): void
useCaptchaOnRegister(): boolean
setCaptchaVisibilityOnRegister(): void

## Used by

- [[account-module]]
- [[login-component]]
- [[register-component]]
- [[register-tenant-component]]
- [[reset-password-component]]
- [[validate-two-factor-code-component]]
- [[session-lock-screen-component]]

## External dependencies

- @angular/core
- abp-ng2-module
- ng-recaptcha

## Notes

Badge visibility is toggled by direct DOM class manipulation rather than Angular binding, which is necessary because the reCAPTCHA badge is rendered outside Angular's component tree.
