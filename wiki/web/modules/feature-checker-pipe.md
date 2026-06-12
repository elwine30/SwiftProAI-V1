---
title: "FeatureCheckerPipe"
type: class
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/shared/common/pipes/feature-checker.pipe.ts"
updated: 2026-06-12
---

# FeatureCheckerPipe

Angular pipe that returns a boolean indicating whether a named ABP feature flag is enabled for the current tenant.

## Public interface

- `transform(feature: string): boolean`

## Used by

- [[utils-module]]

## External dependencies

- `@angular/core`
- `abp-ng2-module`
