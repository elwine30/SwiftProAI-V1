---
title: "HostSettingsComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/admin/settings/host-settings.component.ts"
updated: 2026-06-12
---

# HostSettingsComponent

Host-level system settings page covering SMTP, user management, security, timezone, and external login providers (Facebook, Google, WS-Federation, OpenID Connect) with OIDC response type checkboxes.

## Public interface

loadHostSettings(): void
saveAll(): void
sendTestEmail(): void

## External dependencies

- @angular/core
- @angular/forms

## Notes

Reads `abp.clock.provider.supportsMultipleTimezone` to conditionally display timezone selection. OpenID Connect response type is stored as a comma-delimited string and split into boolean flags for the UI.
