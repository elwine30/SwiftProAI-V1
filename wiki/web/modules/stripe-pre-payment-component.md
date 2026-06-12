---
title: "StripePrePaymentComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/account/payment/stripe/stripe-pre-payment.component.ts"
updated: 2026-06-12
---

# StripePrePaymentComponent

Loads the Stripe.js SDK, creates a checkout session, and attaches a click listener to a button that redirects to the Stripe hosted checkout page.

## Public interface

paymentId: any
payment: SubscriptionPaymentDto
stripeIsLoading: boolean

## External dependencies

- @angular/core
- @angular/router

## Notes

Uses ScriptLoaderService to dynamically inject stripe.js v3 then attaches a raw DOM event listener (not Angular binding) to the checkout button, which is an anti-pattern but required by Stripe's redirect flow.
