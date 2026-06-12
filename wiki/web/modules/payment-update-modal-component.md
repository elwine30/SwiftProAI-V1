---
title: "PaymentUpdateModalComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/main/dashboard/payment-update-modal.component.ts"
updated: 2026-06-12
---

# PaymentUpdateModalComponent

Modal launched from the dashboard to update payment mode and amount-paid fields on a case invoice.

## Public interface

@Output() modalSave: EventEmitter<any>
show(id?): void
save(): void
close(): void

## Used by

- [[dashboard-component]]

## External dependencies

- @angular/core
- ngx-bootstrap/modal
- rxjs/operators

## Notes

Auto-populates amountPaid from totalAmount of the existing invoice on open but does not persist this until save() is called.
