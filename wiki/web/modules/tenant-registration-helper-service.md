---
title: "TenantRegistrationHelperService"
type: service
language: typescript
layer: application
path: "SwiftProAI.Web/angular/src/account/register/tenant-registration-helper.service.ts"
updated: 2026-06-12
---

# TenantRegistrationHelperService

Acts as a transient state holder, passing the RegisterTenantOutput result between the registration and result-display route.

## Public interface

registrationResult: RegisterTenantOutput

## Used by

- [[account-module]]
- [[register-tenant-component]]
- [[register-tenant-result-component]]

## External dependencies

- @angular/core

## Notes

No constructor logic. Relies on Angular's DI singleton lifetime within AccountModule to preserve state across a navigation. If the user refreshes, registrationResult is lost.
