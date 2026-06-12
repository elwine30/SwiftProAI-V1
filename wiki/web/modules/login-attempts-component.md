---
title: "LoginAttemptsComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/admin/login-attempts/login-attempts.component.ts"
updated: 2026-06-12
---

# LoginAttemptsComponent

Security monitoring page listing user login attempts with date range and result-type filters, defaulting to the current week.

## Public interface

getLoginAttempts(event?: LazyLoadEvent): void

## External dependencies

- @angular/core
- luxon
- primeng/api
- primeng/table
- primeng/paginator
- rxjs/operators
