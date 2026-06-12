---
title: "AppSessionService"
type: service
language: typescript
layer: application
path: "SwiftProAI.Web/angular/src/shared/common/session/app-session.service.ts"
updated: 2026-06-12
---

# AppSessionService

Holds the authenticated user's session state (user, tenant, impersonation, theme) and provides the init() call used by APP_INITIALIZER to hydrate state from the backend.

## Public interface

- `init(): Promise<UiCustomizationSettingsDto>`
- `get user(): UserLoginInfoDto`
- `get userId(): number`
- `get tenant(): TenantLoginInfoDto`
- `get tenantId(): number`
- `get tenancyName(): string`
- `get impersonatorUser(): UserLoginInfoDto`
- `get/set theme(): UiCustomizationSettingsDto`
- `getShownLoginName(): string`
- `changeTenantIfNeeded(tenantId?): boolean`

## Used by

- [[auth-route-guard]]
- [[app-component-base]]
- [[common-module]]
- [[app-url-service]]

## External dependencies

- `@angular/core`
- `abp-ng2-module`

## Notes

changeTenantIfNeeded() calls abp.multiTenancy.setTenantIdCookie() then location.reload() — a hard page reload is used to switch tenants. init() is called in APP_INITIALIZER; the resolved UiCustomizationSettingsDto is used to bootstrap theme selection.
