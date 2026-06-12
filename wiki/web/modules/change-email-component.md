---
title: "ChangeEmailComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/account/change-email/change-email.component.ts"
updated: 2026-06-12
---

# ChangeEmailComponent

Processes the email change confirmation link, resolves the tenant context, applies the change, and then logs the user out so they can sign in with the new address.

## Public interface

parseTenantId(tenantIdAsStr?): number
waitMessage: string

## External dependencies

- @angular/core
- @angular/router

## Notes

Structurally identical to ConfirmEmailComponent but calls accountService.changeEmail instead of activateEmail. Both components use inline templates rather than separate HTML files.
