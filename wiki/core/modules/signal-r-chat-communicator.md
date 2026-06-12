---
title: "SignalRChatCommunicator"
type: service
language: csharp
layer: infrastructure
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/Chat/SignalR/SignalRChatCommunicator.cs"
updated: 2026-06-12
---

# SignalRChatCommunicator

Implements IChatCommunicator by pushing real-time events (messages, friendship requests, presence changes, read receipts) to specific SignalR connection IDs via IHubContext.

## Public interface

Task SendMessageToClient(IReadOnlyList<IOnlineClient> clients, ChatMessage message)
Task SendFriendshipRequestToClient(IReadOnlyList<IOnlineClient> clients, Friendship friendship, bool isOwnRequest, bool isFriendOnline)
Task SendUserConnectionChangeToClients(IReadOnlyList<IOnlineClient> clients, UserIdentifier user, bool isConnected)
Task SendUserStateChangeToClients(IReadOnlyList<IOnlineClient> clients, UserIdentifier user, FriendshipState newState)
Task SendAllUnreadMessagesOfUserReadToClients(IReadOnlyList<IOnlineClient> clients, UserIdentifier user)
Task SendReadStateChangeToClients(IReadOnlyList<IOnlineClient> clients, UserIdentifier user)

## Dependencies

- [[chat-hub]] IHubContext<ChatHub> is injected to obtain the SignalR client proxy

## Used by

- [[web-core-module]] registered as the IChatCommunicator implementation during module initialisation

## External dependencies

- Microsoft.AspNetCore.SignalR
- Abp.Dependency
