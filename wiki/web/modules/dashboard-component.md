---
title: "DashboardComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/main/dashboard/dashboard.component.ts"
updated: 2026-06-12
---

# DashboardComponent

Main case management dashboard showing a paginated, filterable registration list with status summary counts and action modals for reassignment and payment.

## Public interface

getMainRegistrationDetails(event?): void — loads paginated case list from API
getDashboardSummary(): void — fetches status-bucketed case counts
createNewRegistration(): void — navigates to registration form
reasignCaseCompany(registerId): void — opens company reassignment modal
reasignCaseAdjuster(registerId): void — opens adjuster reassignment modal
paymentUpdate(id): void — opens payment update modal
onClick(statusId): void — filters list by status via query params
onRowClicked(id, isView): void — navigates into case via NavigationService
exportTable(): void — CSV export via PrimeNG Table
isReassignable(statusId): boolean

## Dependencies

- [[create-registration-modal-component]] modal for creating a new case registration
- [[reasign-company-modal-component]] modal for transferring a case to a different insurance company
- [[reasign-adjuster-modal-component]] modal for reassigning the adjuster on an existing case
- [[payment-update-modal-component]] modal for updating payment mode and amount on a case invoice

## External dependencies

- @angular/core
- @angular/router
- @angular/common
- primeng/api
- primeng/table
- primeng/paginator
- rxjs/operators
- luxon
- lodash-es

## Notes

Reads filter state from query params on init, enabling back-navigation to a pre-filtered view. Extends AppComponentBase (ABP base class providing primengTableHelper, notify, l() i18n). Uses ViewEncapsulation.None.
