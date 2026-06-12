---
title: "GatewaySelectionComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/account/payment/gateway-selection.component.ts"
updated: 2026-06-12
---

# GatewaySelectionComponent

Displays available payment gateways for a pending subscription payment and routes to the chosen gateway's pre-payment page.

## Public interface

checkout(gatewayType): void
getPaymentGatewayType(gatewayType): string
paymentGateways: PaymentGatewayModel[]
recurringPaymentEnabled: boolean

## Dependencies

- [[payment-helper-service]] translates the numeric gateway type enum to a route path string

## Used by

- [[payment-helper-service]]

## External dependencies

- @angular/core
- @angular/router

## Notes

Calls abp.multiTenancy.setTenantIdCookie with the tenantId from the payment DTO to ensure the correct tenant context during checkout.
