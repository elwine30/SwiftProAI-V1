---
title: "StripePostPaymentComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/account/payment/stripe/stripe-post-payment.component.ts"
updated: 2026-06-12
---

# StripePostPaymentComponent

Polls the backend for Stripe payment confirmation after Stripe redirects back, using exponential back-off up to five attempts.

## Public interface

getPaymentResult(): void
controlAgain(): void
paymentId: number

## External dependencies

- @angular/core
- @angular/router

## Notes

Back-off doubles the controlTimeout on each retry (starting at 5 s, max 5 retries) via setTimeout rather than rxjs operators.
