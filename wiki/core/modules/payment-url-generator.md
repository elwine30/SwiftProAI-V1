---
title: "PaymentUrlGenerator"
type: service
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Host/Url/PaymentUrlGenerator.cs"
updated: 2026-06-12
---

# PaymentUrlGenerator

Generates the Angular client URL for the payment gateway selection page given a subscription payment record.

## Public interface

- `string CreatePaymentRequestUrl(SubscriptionPayment subscriptionPayment)`

## Dependencies

- [[web-url-service]] resolves the client root address used as the base for the generated payment URL

## External dependencies

- Abp.Dependency
