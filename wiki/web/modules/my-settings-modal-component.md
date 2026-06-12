---
title: "MySettingsModalComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/shared/layout/profile/my-settings-modal.component.ts"
updated: 2026-06-12
---

# MySettingsModalComponent

Modal for editing the current user's profile, including timezone, two-factor authentication and phone verification.

## Public interface

- `@Output() modalSave: EventEmitter<any>`
- `show(): void`
- `save(): void`
- `close(): void`
- `smsVerify(): void`
- `enableTwoFactorAuthentication(): void`
- `disableTwoFactorAuthentication(verifyCodeInput): void`
- `changePhoneNumberToVerified(): void`

## External dependencies

- `@angular/core`
- `ngx-bootstrap/modal`
- `rxjs`

## Notes

Reads abp.setting for 2FA and multi-timezone flags at runtime. Triggers a full page reload via message.info callback if the timezone changes.
