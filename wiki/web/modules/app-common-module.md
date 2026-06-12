---
title: "AppCommonModule"
type: module
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/app/shared/common/app-common.module.ts"
updated: 2026-06-12
---

# AppCommonModule

Declares and exports shared UI components, directives and form helpers used across both main and admin feature modules; also provides core services via forRoot().

## Public interface

forRoot(): ModuleWithProviders — provides AppAuthService and AppRouteGuard
exports: TimeZoneComboComponent, CommonLookupModalComponent, StepNavComponent, ActionsButtonComponent, EntityTypeHistoryModalComponent, DateRangePickerInitialValueSetterDirective, DatePickerInitialValueSetterDirective, PasswordInputWithShowButtonComponent, KeyValueListManagerComponent, CreateRemarkModalComponent, AddRemarkButtonComponent, OnlyNumberDirective, AmountInputDirective, CreateAndViewExpensesModalComponent, DecimalFormatDirective

## Dependencies

- [[app-localization-service]] provides ABP localisation helpers to the module
- [[app-navigation-service]] builds the full permission-filtered menu tree
- [[app-auth-service]] handles user logout and token clearing
- [[auth-route-guard]] protects authenticated routes and validates ABP permissions
- [[date-time-service]] provides timezone-aware date formatting helpers
- [[step-nav-component]] multi-step wizard navigation component
- [[actions-button-component]] reusable row-level actions dropdown button
- [[add-remark-button-component]] inline button for opening the remark modal
- [[only-number-directive]] input directive restricting entry to numeric characters
- [[amount-input-directive]] input directive formatting currency amounts
- [[decimal-format-directive]] input directive enforcing decimal precision

## Used by

- [[app-shared-module]]

## External dependencies

- @angular/core
- @angular/common
- @angular/forms
- ngx-bootstrap/modal
- ngx-bootstrap/datepicker
- ngx-bootstrap/dropdown
- ngx-bootstrap/tabs
- primeng/table
- primeng/paginator
- primeng/autocomplete
- @craftsjs/perfect-scrollbar
- @awaismirza/angular2-counto

## Notes

Uses ModuleWithProviders pattern for forRoot(). DatepickerConfig providers use factory functions from a custom NgxBootstrapDatePickerConfigService asset.
