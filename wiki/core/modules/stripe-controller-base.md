---
title: "StripeControllerBase"
type: class
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/Controllers/StripeControllerBase.cs"
updated: 2026-06-12
---

# StripeControllerBase

Abstract base controller that handles incoming Stripe webhook events, routing invoice.paid (subscription cycle) and checkout.session.completed events to the appropriate payment service.

## Public interface

Task<IActionResult> WebHooks()

## Dependencies

- [[thinkninsurtech-controller-base]] base class providing localisation and Identity helpers

## External dependencies

- Stripe.net
