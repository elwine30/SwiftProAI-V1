---
title: "SelectEditionComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/account/register/select-edition.component.ts"
updated: 2026-06-12
---

# SelectEditionComponent

Displays available subscription editions and their features in a pricing-table layout before the user proceeds to tenant registration.

## Public interface

isFree(edition: EditionSelectDto): boolean
isTrueFalseFeature(feature: FlatFeatureSelectDto): boolean
featureEnabledForEdition(feature, edition): boolean
getFeatureValueForEdition(feature, edition): string
changePaymentPeriodType(paymentPeriodType: string): void

## External dependencies

- @angular/core
- @angular/router
- lodash-es

## Notes

Calls the global KTApp.init() in ngAfterViewInit to initialise Metronic JavaScript components after Angular renders the pricing table.
