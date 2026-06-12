---
title: "ISubscriptionAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/MultiTenancy/ISubscriptionAppService.cs"
updated: 2026-06-12
---

# ISubscriptionAppService

Contract for managing subscription lifecycle transitions including upgrades, extensions and trial-to-paid conversions.

## Public interface

Task DisableRecurringPayments()
Task EnableRecurringPayments()
Task<long> StartExtendSubscription(StartExtendSubscriptionInput input)
Task<StartUpgradeSubscriptionOutput> StartUpgradeSubscription(StartUpgradeSubscriptionInput input)
Task<long> StartTrialToBuySubscription(StartTrialToBuySubscriptionInput input)

## External dependencies

- Abp.Core
