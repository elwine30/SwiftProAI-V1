---
title: "TenantSettingsComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/admin/settings/tenant-settings.component.ts"
updated: 2026-06-12
---

# TenantSettingsComponent

Tenant-level settings page extending host settings with logo upload (dark/light, full/minimal) and custom CSS injection via ng2-file-upload FileUploader instances.

## Public interface

loadSettings(): void
saveAll(): void
sendTestEmail(): void

## External dependencies

- @angular/core
- @angular/forms
- abp-ng2-module
- ng2-file-upload
- rxjs/operators

## Notes

Uses ng2-file-upload FileUploader (not PrimeNG) for logo assets. ABP TokenService is injected to attach bearer tokens to file upload requests. Reads `abp.clock.provider.supportsMultipleTimezone` for conditional tab ordering.
