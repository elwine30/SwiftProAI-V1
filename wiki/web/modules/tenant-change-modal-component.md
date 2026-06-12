---
title: "TenantChangeModalComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/account/shared/tenant-change-modal.component.ts"
updated: 2026-06-12
---

# TenantChangeModalComponent

Bootstrap modal that lets an unauthenticated user switch between tenants or revert to the host context before logging in.

## Public interface

show(tenancyName: string): void
save(): void
close(): void
switchToTenant(e): void
onShown(): void

## Used by

- [[account-shared-module]]
- [[tenant-change-component]]

## External dependencies

- @angular/core
- ngx-bootstrap/modal
- rxjs

## Notes

Calls abp.multiTenancy.setTenantIdCookie and location.reload() on confirmation, relying on full-page reload to re-initialise ABP session with the new tenant.
