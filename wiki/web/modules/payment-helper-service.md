---
title: "PaymentHelperService"
type: service
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/account/payment/payment-helper.service.ts"
updated: 2026-06-12
---

# PaymentHelperService

Translates a numeric gateway type enum value to a human-readable string used to construct gateway-specific route paths.

## Public interface

getPaymentGatewayType(gatewayType): string

## Used by

- [[account-module]]
- [[gateway-selection-component]]

## External dependencies

- @angular/core
