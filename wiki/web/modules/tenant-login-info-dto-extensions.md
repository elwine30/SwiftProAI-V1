---
title: "TenantLoginInfoDtoExtensions"
type: class
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/shared/service-proxies/tenant-login-info-dto-extensions.ts"
updated: 2026-06-12
---

# TenantLoginInfoDtoExtensions

Extends the generated TenantLoginInfoDto class with logo-presence helper methods using TypeScript module augmentation.

## Public interface

- `HasLogo(): boolean`
- `HasLogoMinimal(): boolean`
- `HasDarkLogo(): boolean`
- `HasLightLogo(): boolean`

## Used by

- [[app-component-base]]

## Notes

Uses TypeScript declaration merging (declare module) to add methods to the auto-generated DTO without modifying the generated file. Imported as a side-effect import in AppComponentBase.
