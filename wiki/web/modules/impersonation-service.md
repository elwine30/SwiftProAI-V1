---
title: "ImpersonationService"
type: service
language: typescript
layer: application
path: "SwiftProAI.Web/angular/src/app/admin/users/impersonation.service.ts"
updated: 2026-06-12
---

# ImpersonationService

Handles all flavours of user impersonation — by tenant, by user within a tenant, via delegated user delegation — by obtaining an impersonation token from the backend and redirecting the browser to the target tenant URL.

## Public interface

impersonateTenant(userId: number, tenantId?: number): void
impersonateUser(userId: number, tenantId: number): void
delegatedImpersonate(userDelegationId: number, tenantId?: number): void
backToImpersonator(): void
getAppRootUrl(result: ImpersonateOutput): string

## Used by

- [[users-component]]
- [[users-module]]
- [[tenants-component]]

## External dependencies

- @angular/core

## Notes

Uses `abp.session.impersonatorTenantId` from the global ABP JS namespace to build the return URL in backToImpersonator(). Logout with redirect target is the impersonation mechanism — no cookie swap.
