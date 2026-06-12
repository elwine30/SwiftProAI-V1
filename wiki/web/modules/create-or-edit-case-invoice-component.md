---
title: "CreateOrEditCaseInvoiceComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/main/registration/caseInvoices/create-or-edit-caseInvoice.component.ts"
updated: 2026-06-12
---

# CreateOrEditCaseInvoiceComponent

Full invoice authoring form with per-line-item SST toggle, mileage/photo quantity calculations, total text-form conversion, and invoice preview navigation.

## Public interface

show(registerId?): void — loads invoice and claim data
save(): void — persists invoice
preview(): void — saves then navigates to invoice preview
onItemAmountChange(item): void — recalculates SST for changed line
toggleSST(e): void — enables/disables SST across all lines
onMileageKMChange(): void
onPhotographQtyChange(): void
updateInvoiceItemTotal(): void — fetches typed expense totals from API
goBack(): void

## Dependencies

- [[invoice-items-component]] renders the line-item list within the invoice form
- [[create-or-edit-invoice-item-modal-component]] modal for adding or editing individual invoice line items

## Used by

- [[invoice-items-component]]
- [[create-or-edit-invoice-item-modal-component]]

## External dependencies

- @angular/core
- @angular/forms
- @angular/common
- @angular/router
- rxjs/operators
- luxon
- ngx-bootstrap/modal

## Notes

Uses NumberToWordsPipe to render the total as a text string for invoice printing. Per-item SST checkbox state is tracked in sstCheckboxStates map. InvoiceItemsServiceProxy is called separately to aggregate typed expense totals (ThirdParty, SearchFee, AirFare etc.) into invoice fields.
