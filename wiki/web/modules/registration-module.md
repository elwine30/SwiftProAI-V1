---
title: "RegistrationModule"
type: module
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/app/main/registration/registration.module.ts"
updated: 2026-06-12
---

# RegistrationModule

Feature module for the top-level registration step, importing AppSharedModule and AppCommonModule.

## Dependencies

- [[registration-routing-module]] defines the route for the registration component

## Used by

- [[registration-routing-module]]

## External dependencies

- @angular/core

## Notes

Thin module — all sub-feature modules (caseAdjusters, caseClaims etc.) are separately lazy loaded via MainRoutingModule.
