---
title: "MainModule"
type: module
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/app/main/main.module.ts"
updated: 2026-06-12
---

# MainModule

Root lazy-loaded feature module for all user-facing areas; configures ngx-bootstrap datepicker providers and imports shared/utility modules for the main area.

## Dependencies

- [[main-routing-module]] defines all lazy-loaded child routes for the main area

## External dependencies

- @angular/common
- @angular/forms
- ngx-bootstrap/modal
- ngx-bootstrap/tabs
- ngx-bootstrap/tooltip
- ngx-bootstrap/dropdown
- ngx-bootstrap/popover
- ngx-bootstrap/datepicker
- @awaismirza/angular2-counto

## Notes

Declares no components itself — all feature components are declared in their own child lazy modules. Uses factory providers via NgxBootstrapDatePickerConfigService to configure ABP-aware datepicker locale.
