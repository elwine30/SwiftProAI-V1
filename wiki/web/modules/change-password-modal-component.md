---
title: "ChangePasswordModalComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/shared/layout/profile/change-password-modal.component.ts"
updated: 2026-06-12
---

# ChangePasswordModalComponent

Modal dialog that allows authenticated users to change their password with complexity rules fetched from the profile API.

## Public interface

- `show(): void`
- `close(): void`
- `save(): void`

## External dependencies

- `@angular/core`
- `ngx-bootstrap/modal`
- `rxjs`

## Notes

Bootstraps Metronic PasswordMeterComponent directly via its static method in onShown().
