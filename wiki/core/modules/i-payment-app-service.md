---
title: "IPaymentAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/MultiTenancy/Payments/IPaymentAppService.cs"
updated: 2026-06-12
---

# IPaymentAppService

Contract for creating, cancelling and querying subscription payments and retrieving active payment gateway configurations.

## Public interface

Task<long> CreatePayment(CreatePaymentDto input)
Task CancelPayment(CancelPaymentDto input)
Task UpdatePayment(UpdatePaymentDto input)
Task<PagedResultDto<SubscriptionPaymentListDto>> GetPaymentHistory(GetPaymentHistoryInput input)
List<PaymentGatewayModel> GetActiveGateways(GetActiveGatewaysInput input)
Task<SubscriptionPaymentDto> GetPaymentAsync(long paymentId)
Task<SubscriptionPaymentDto> GetLastCompletedPayment()
Task PaymentFailed(long paymentId)
Task<bool> HasAnyPayment()

## External dependencies

- Abp.Core
