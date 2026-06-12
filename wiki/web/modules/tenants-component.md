---
title: "TenantsComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/admin/tenants/tenants.component.ts"
updated: 2026-06-12
---

# TenantsComponent

Host-only paginated tenant list with subscription and creation date range filters, supporting create/edit/delete, feature management, entity history, and user impersonation into a tenant.

## Public interface

getTenants(event?: LazyLoadEvent): void
createTenant(): void
deleteTenant(tenant: TenantListDto): void
showHistory(tenant: TenantListDto): void
impersonateUser(item: FindUsersOutputDto): void
unlockUser(record: any): void
reloadPage(): void

## Dependencies

- [[create-tenant-modal-component]] modal for provisioning a new tenant
- [[edit-tenant-modal-component]] modal for editing an existing tenant's settings
- [[tenant-features-modal-component]] modal for managing feature flags per tenant
- [[impersonation-service]] handles browser redirect for impersonating a user within a tenant

## Used by

- [[impersonation-service]]

## External dependencies

- @angular/core
- @angular/router
- luxon
- primeng/api
- primeng/table
- primeng/paginator
- rxjs/operators
- lodash-es

## Notes

Calls `abp.multiTenancy.getTenantIdCookie()` to guard impersonation — host users must not already be impersonating another tenant. Route query params seed the date-range filters on load.
