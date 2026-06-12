---
title: "SubscriptionPaymentRepository"
type: repository
language: csharp
layer: data
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.EntityFrameworkCore/MultiTenancy/Payments/SubscriptionPaymentRepository.cs"
updated: 2026-06-12
---

# SubscriptionPaymentRepository

Repository for SubscriptionPayment that provides eager-load, gateway-lookup, and last-payment queries needed by the subscription and billing workflows.

## Public interface

Task<SubscriptionPayment> GetPaymentWithProducts(long id)
Task<SubscriptionPayment> GetByGatewayAndPaymentIdAsync(SubscriptionPaymentGatewayType, string)
Task<SubscriptionPayment> GetLastCompletedPaymentOrDefaultAsync(int, SubscriptionPaymentGatewayType?, bool?)
Task<SubscriptionPayment> GetLastPaymentOrDefaultAsync(int, SubscriptionPaymentGatewayType?, bool?)

## Dependencies

- [[thinkn-insur-tech-repository-base]] base repository providing DbContext access
- [[thinkn-insur-tech-db-context]] DbSet access for SubscriptionPayment queries

## Used by

- [[thinkn-insur-tech-db-context]]

## External dependencies

- Abp.EntityFrameworkCore
- Abp.Linq.Extensions

## Notes

GetLastCompletedPaymentOrDefaultAsync materialises the entire filtered list to memory before OrderByDescending, which may be inefficient for large data sets.
