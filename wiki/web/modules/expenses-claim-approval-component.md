---
title: "ExpensesClaimApprovalComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/admin/expenses-claim-approval/expenses-claim-approval.component.ts"
updated: 2026-06-12
---

# ExpensesClaimApprovalComponent

Admin approval queue for expenses and claims, allowing bulk approve/reject actions per row with in-memory state tracking before a single batch save call.

## Public interface

getTableData(event?: LazyLoadEvent): void
save(): void
actionOnChange(event: any, id: number): void
onGroupChange(value: string): void
goToEdit(record: any): void
isShouldHide(statusCode: string): boolean

## External dependencies

- @angular/common
- @angular/core
- @angular/router
- ngx-bootstrap/modal
- primeng/api
- primeng/table
- primeng/paginator
- luxon
- rxjs

## Notes

Approval state (approved/rejected booleans) is stored directly on the DTO objects in-memory rather than in a separate map. Bug: `item.approved == false` (equality check, not assignment) in the reject branch. Opens expenses in a ngx-bootstrap modal but claims via router navigation.
