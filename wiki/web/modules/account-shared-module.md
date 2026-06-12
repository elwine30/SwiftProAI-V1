---
title: "AccountSharedModule"
type: module
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/account/shared/account-shared.module.ts"
updated: 2026-06-12
---

# AccountSharedModule

Exports the tenant-change component pair so they can be consumed by the account shell template.

## Public interface

exports: [TenantChangeComponent, TenantChangeModalComponent]

## Dependencies

- [[tenant-change-component]] inline widget that displays current tenant name and opens the modal
- [[tenant-change-modal-component]] modal for switching tenants before login

## Used by

- [[account-module]]

## External dependencies

- @angular/core
