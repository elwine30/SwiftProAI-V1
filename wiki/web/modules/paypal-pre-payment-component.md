---
title: "PayPalPrePaymentComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/account/payment/paypal/paypal-pre-payment.component.ts"
updated: 2026-06-12
---

# PayPalPrePaymentComponent

Loads the PayPal SDK, renders the PayPal button, and on approval confirms the payment with the backend before navigating to the success callback URL.

## Public interface

preparePaypalButton(): void
GetDisabledFundingsQueryString(config): string
setTenantIdCookieIfNeeded(): void
payment: SubscriptionPaymentDto

## External dependencies

- @angular/core
- @angular/router

## Notes

Appends client-id, currency and disable-funding query parameters to the PayPal SDK URL before dynamic script injection. Uses (window as any).paypal.Buttons, coupling the component to the globally loaded SDK.
