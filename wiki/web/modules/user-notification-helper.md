---
title: "UserNotificationHelper"
type: service
language: typescript
layer: application
path: "SwiftProAI.Web/angular/src/app/shared/layout/notifications/UserNotificationHelper.ts"
updated: 2026-06-12
---

# UserNotificationHelper

Formats ABP user-notification objects into display DTOs, shows in-app and desktop push notifications and manages mark-as-read interactions.

## Public interface

- `settingsModal: NotificationSettingsModalComponent (set by host)`
- `format(userNotification, truncateText?): IFormattedUserNotification`
- `show(userNotification): void`
- `getUrl(userNotification): string`
- `getUiIconBySeverity(severity): string`
- `setAllAsRead(callback?): void`
- `setAsRead(userNotificationId, callback?): void`
- `openSettingsModal(): void`
- `shouldUserUpdateApp(): void`

## Dependencies

- [[date-time-service]] formats notification timestamps for display

## Used by

- [[header-notifications-component]]

## External dependencies

- `@angular/core`
- `@angular/common/http`
- `push.js`
- `luxon`

## Notes

Uses push.js for desktop notifications. shouldUserUpdateApp() polls a custom BrowserCacheCleaner endpoint and uses ABP confirm dialog for the upgrade prompt.
