---
title: "HeaderNotificationsComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/shared/layout/notifications/header-notifications.component.ts"
updated: 2026-06-12
---

# HeaderNotificationsComponent

Bell-icon dropdown in the app header that displays recent notifications, unread count and delegates mark-as-read actions to UserNotificationHelper.

## Public interface

- `@Input() customStyle: string`
- `@Input() iconStyle: string`
- `@Input() isRight: boolean`
- `loadNotifications(): void`
- `setAllNotificationsAsRead(): void`
- `setNotificationAsRead(userNotification): void`
- `openNotificationSettingsModal(): void`
- `gotoUrl(url): void`

## Dependencies

- [[user-notification-helper]] formats notifications and handles mark-as-read operations

## External dependencies

- `@angular/core`
- `lodash-es`

## Notes

Subscribes to abp.notifications.received, app.notifications.refresh and app.notifications.read events via AppComponentBase.subscribeToEvent. Calls shouldUserUpdateApp on each load.
