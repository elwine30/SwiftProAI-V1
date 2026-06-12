---
title: "MainRoutingModule"
type: module
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/app/main/main-routing.module.ts"
updated: 2026-06-12
---

# MainRoutingModule

Defines all lazy-loaded child routes under the main area, each guarded by a permission string matching ABP page permission keys.

## Used by

- [[main-module]]

## External dependencies

- @angular/core
- @angular/router

## Notes

Every route uses dynamic import() for code-splitting. Permission strings follow the ABP Pages.* convention (e.g. Pages.Tenant.Dashboard). Note caseStakeholders route incorrectly maps to Pages.CaseInsurers permission — likely a copy-paste oversight.
