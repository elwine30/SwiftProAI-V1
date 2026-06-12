---
title: "CreateOrEditCaseClaimComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/main/registration/caseClaims/create-or-edit-caseClaim.component.ts"
updated: 2026-06-12
---

# CreateOrEditCaseClaimComponent

Editable claim form that calculates mileage and expense totals in-browser, manages claim submission workflow, and broadcasts status changes via CaseClaimDataService.

## Public interface

show(registerId?): void — loads existing claim data
save(): void — persists claim
submit(): void — transitions claim to PendingForApproval status
updateTotal(): void — recalculates total from all expense fields
onMileageKMChange(): void
updateSearchfeeTotal(): void — fetches search fee sum from API
canDeactivate(): boolean
createCaseSearchFee(): void

## Dependencies

- [[case-claim-data-service]] broadcasts the current claim statusId to sibling components
- [[create-or-edit-case-search-fee-modal-component]] modal for adding search fees to the claim
- [[case-search-fees-component]] displays the list of search fees on the claim

## Used by

- [[case-claim-data-service]]

## External dependencies

- @angular/core
- @angular/forms
- @angular/common
- @angular/router
- primeng/paginator
- rxjs/operators
- rxjs

## Notes

Uses CaseClaimDataService (BehaviorSubject) to propagate statusId to sibling components. Implements DirtyFormGuard. Has a bug in the show() method: the else-branch initialising a new claim uses incorrect null-check logic (checks !caseClaim after already accessing result.caseClaim).
