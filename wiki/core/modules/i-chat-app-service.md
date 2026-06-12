---
title: "IChatAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Chat/IChatAppService.cs"
updated: 2026-06-12
---

# IChatAppService

Contract for the real-time chat feature — retrieving friend lists, loading message history and marking messages as read.

## Public interface

Task<GetUserChatFriendsWithSettingsOutput> GetUserChatFriendsWithSettings()
Task<ListResultDto<ChatMessageDto>> GetUserChatMessages(GetUserChatMessagesInput input)
Task MarkAllUnreadMessagesOfUserAsRead(MarkAllUnreadMessagesOfUserAsReadInput input)

## External dependencies

- Abp.Core

## Notes

REST surface only — real-time delivery is handled via SignalR hubs in ThinknInsurTech.Web.Core, not through this interface.
