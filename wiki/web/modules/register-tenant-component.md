---
title: "RegisterTenantComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/account/register/register-tenant.component.ts"
updated: 2026-06-12
---

# RegisterTenantComponent

Collects new-tenant registration details, submits them, and routes to gateway selection for paid subscriptions or directly to the result page for free plans.

## Public interface

save(): void
model: RegisterTenantModel
passwordComplexitySetting: PasswordComplexitySetting
selectedPaymentPeriodType: PaymentPeriodType

## Dependencies

- [[tenant-registration-helper-service]] stores the registration output for the result page
- [[recaptchav3-wrapper-service]] enforces reCAPTCHA on the tenant registration form

## Used by

- [[tenant-registration-helper-service]]

## External dependencies

- @angular/core
- @angular/router
- rxjs

## Notes

Sets successUrl and errorUrl on the model using abp.appPath, relying on the global ABP namespace rather than AppConsts.
