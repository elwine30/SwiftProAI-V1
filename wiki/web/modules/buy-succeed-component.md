---
title: "BuySucceedComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/account/buy-succeed/buy-succeed.component.ts"
updated: 2026-06-12
---

# BuySucceedComponent

Post-payment landing page that notifies the backend of a successful buy-now payment and then navigates the user to the subscription management admin page.

## External dependencies

- @angular/core
- @angular/router

## Notes

Navigation target uses abp.appPath concatenation rather than a RouterLink or AppConsts, coupling it to the global ABP namespace.
