---
title: "ConfirmEmailComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/account/email-activation/confirm-email.component.ts"
updated: 2026-06-12
---

# ConfirmEmailComponent

Processes the email confirmation link, resolves the correct tenant, activates the email address, and logs out the current session if one is active.

## Public interface

parseTenantId(tenantIdAsStr?): number
waitMessage: string

## External dependencies

- @angular/core
- @angular/router

## Notes

Calls appSession.changeTenantIfNeeded before activating the email to handle cross-tenant confirmation links. Uses AppAuthService for logout to ensure token cleanup.
