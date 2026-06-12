---
title: "RegisterTenantResultComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/account/register/register-tenant-result.component.ts"
updated: 2026-06-12
---

# RegisterTenantResultComponent

Displays the outcome of tenant registration and provides a link to the newly created tenant's URL.

## Public interface

model: RegisterTenantOutput
tenantUrl: string

## Dependencies

- [[tenant-registration-helper-service]] provides the RegisterTenantOutput from the prior registration step

## Used by

- [[tenant-registration-helper-service]]

## External dependencies

- @angular/core
- @angular/router

## Notes

Redirects to login if no registration result is held in TenantRegistrationHelperService, preventing direct URL access.
