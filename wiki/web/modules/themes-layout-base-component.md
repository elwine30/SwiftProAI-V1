---
title: "ThemesLayoutBaseComponent"
type: component
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/app/shared/layout/themes/themes-layout-base.component.ts"
updated: 2026-06-12
---

# ThemesLayoutBaseComponent

Abstract base class for all 13 theme layout components providing subscription status, trial period and dark-mode helpers.

## Public interface

- `subscriptionStatusBarVisible(): boolean`
- `subscriptionIsExpiringSoon(): boolean`
- `getSubscriptionExpiringDayCount(): number`
- `getTrialSubscriptionNotification(): string`
- `getExpireNotification(localizationKey: string): string`
- `isMobileDevice(): boolean`
- `isDarkModeActive(): boolean`
- `getSkin(): string`

## Dependencies

- [[date-time-service]] used for subscription expiry date calculations

## Used by

- [[app-shared-module]]
- [[default-layout-component]]

## External dependencies

- `@angular/core`

## Notes

Uses KTUtil global from Metronic for mobile detection. defaultLogo path is constructed at class-field init time referencing this.currentTheme, which is injected by AppComponentBase.
