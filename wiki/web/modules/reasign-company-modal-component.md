---
title: "ReasignCompanyModalComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/main/dashboard/reasign-company-modal.component.ts"
updated: 2026-06-12
---

# ReasignCompanyModalComponent

Modal for transferring a case to a different insurance company, with a mandatory remark captured via ReassignCaseCompanyDto.

## Public interface

@Output() modalSave: EventEmitter<any>
show(registerId): void
save(): void
close(): void

## Used by

- [[dashboard-component]]

## External dependencies

- @angular/core
- ngx-bootstrap/modal
- rxjs/operators

## Notes

Has a potential race condition on open: getAllAdjusterByBranchForTableDropdown is called before the getMainRegistrationDetailsByRegisterId callback returns, so branchId may be undefined on first open.
