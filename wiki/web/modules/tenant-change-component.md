---
title: "TenantChangeComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/account/shared/tenant-change.component.ts"
updated: 2026-06-12
---

# TenantChangeComponent

Inline widget shown in the account header that displays the current tenant name and opens the tenant-change modal.

## Public interface

showChangeModal(): void
get isMultiTenancyEnabled(): boolean
tenancyName: string

## Dependencies

- [[tenant-change-modal-component]] opened by showChangeModal() to perform the actual tenant switch

## Used by

- [[account-shared-module]]

## External dependencies

- @angular/core

## Notes

Uses abp.multiTenancy.isEnabled from the global namespace to conditionally render the widget.
