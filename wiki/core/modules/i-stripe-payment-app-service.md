---
title: "IStripePaymentAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/MultiTenancy/Payments/Stripe/IStripePaymentAppService.cs"
updated: 2026-06-12
---

# IStripePaymentAppService

Stripe-specific contract for creating payment sessions, confirming payments and surfacing Stripe configuration to the client.

## Public interface

Task ConfirmPayment(StripeConfirmPaymentInput input)
StripeConfigurationDto GetConfiguration()
Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input)

## External dependencies

- Abp.Core
