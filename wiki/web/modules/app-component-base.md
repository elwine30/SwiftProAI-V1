---
title: "AppComponentBase"
type: class
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/shared/common/app-component-base.ts"
updated: 2026-06-12
---

# AppComponentBase

Abstract base class that all feature components extend; wires up all common ABP and application services via Injector to avoid repetitive constructor injection.

## Public interface

- `l(key: string, ...args: any[]): string` — localise with default source
- `ls(sourcename, key, ...args): string` — localise with explicit source
- `isGranted(permissionName: string): boolean`
- `isGrantedAny(...permissions: string[]): boolean`
- `s(key: string): string` — read ABP setting
- `showMainSpinner(text?: string): void`
- `hideMainSpinner(): void`
- `get currentTheme(): UiCustomizationSettingsDto`
- `get containerClass(): string`
- `subscribeToEvent(eventName, callback): void` — auto-unsubscribed on destroy

## Dependencies

- [[app-consts]] provides static URL and cookie name constants
- [[app-url-service]] resolves tenant-aware application root URLs
- [[app-session-service]] provides the authenticated user's session state
- [[app-ui-customization-service]] provides active theme CSS class names
- [[primeng-table-helper]] instantiated directly for table pagination/sort state
- [[tenant-login-info-dto-extensions]] imported as side-effect to augment generated DTO with logo helpers

## Used by

- [[auth-route-guard]]
- [[app-component-base]]
- [[common-module]]
- [[app-url-service]]

## External dependencies

- `@angular/core`
- `@angular/forms`
- `abp-ng2-module`
- `ngx-spinner`
- `sweetalert2`

## Notes

Uses Injector pattern rather than direct constructor injection so subclasses do not need to re-declare every service. Exposes swalAlert = Swal directly on the class. Subscribes to abp.event bus and cleans up on ngOnDestroy. markFormAsPristine is a utility to reset dirty-state after save.
