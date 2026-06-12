---
title: "AdminModule"
type: module
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/app/admin/admin.module.ts"
updated: 2026-06-12
---

# AdminModule

Root lazy-loaded feature module for the entire admin area, wiring up third-party UI providers (ngx-bootstrap modals, datepickers, tooltips) and providing the date-picker factory services.

## Dependencies

- [[admin-routing-module]] defines all lazy-loaded child routes for the admin area

## Used by

- [[admin-routing-module]]

## External dependencies

- @angular/core
- primeng/api
- ngx-bootstrap/datepicker
- ngx-bootstrap/modal
- ngx-bootstrap/tabs
- ngx-bootstrap/tooltip
- ngx-bootstrap/popover
- ngx-bootstrap/dropdown

## Notes

Datepicker config is provided via factory methods from NgxBootstrapDatePickerConfigService. No component declarations — all feature sub-modules are lazy-loaded from AdminRoutingModule.
