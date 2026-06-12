---
title: "INotificationAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Notifications/INotificationAppService.cs"
updated: 2026-06-12
---

# INotificationAppService

Contract for managing user notification subscriptions, reading/deleting notifications and broadcasting mass notifications.

## Public interface

Task<GetNotificationsOutput> GetUserNotifications(GetUserNotificationsInput input)
Task SetAllNotificationsAsRead()
Task<SetNotificationAsReadOutput> SetNotificationAsRead(EntityDto<Guid> input)
Task DeleteNotification(EntityDto<Guid> input)
Task DeleteAllUserNotifications(DeleteAllUserNotificationsInput input)
Task CreateMassNotification(CreateMassNotificationInput input)
Task CreateNewVersionReleasedNotification()
Task<bool> ShouldUserUpdateApp()
List<string> GetAllNotifiers()
Task<GetPublishedNotificationsOutput> GetNotificationsPublishedByUser(GetPublishedNotificationsInput input)

## External dependencies

- Abp.Core
- Abp.Notifications
