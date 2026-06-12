---
title: "CookieConsentService"
type: service
language: typescript
layer: application
path: "SwiftProAI.Web/angular/src/shared/common/session/cookie-consent.service.ts"
updated: 2026-06-12
---

# CookieConsentService

Initialises the browser cookie-consent banner when enabled via ABP settings, using localised message strings.

## Public interface

- `init(): void`

## Used by

- [[common-module]]

## External dependencies

- `@angular/core`
- `abp-ng2-module`

## Notes

Reads App.UserManagement.IsCookieConsentEnabled from abp.setting at runtime. Calls window.cookieconsent.initialise() — relies on the cookieconsent script being loaded as a global via angular.json scripts.
