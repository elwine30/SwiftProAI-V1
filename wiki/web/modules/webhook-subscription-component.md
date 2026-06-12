---
title: "WebhookSubscriptionComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/admin/webhook-subscription/webhook-subscription.component.ts"
updated: 2026-06-12
---

# WebhookSubscriptionComponent

Lists all active outbound webhook subscriptions, supports creating new subscriptions via modal, and navigates to the per-subscription event detail route.

## Public interface

getSubscriptions(event?: any): void
createSubscription(): void
goToSubscriptionDetail(subscriptionId: string): void

## Dependencies

- [[create-or-edit-webhook-subscription-modal-component]] modal form for creating or editing a webhook subscription

## Used by

- [[create-or-edit-webhook-subscription-modal-component]]

## External dependencies

- @angular/core
- @angular/router
- rxjs/operators
