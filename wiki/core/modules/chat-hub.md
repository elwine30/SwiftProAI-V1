---
title: "ChatHub"
type: class
language: csharp
layer: infrastructure
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/Chat/SignalR/ChatHub.cs"
updated: 2026-06-12
---

# ChatHub

SignalR hub that receives and routes real-time chat messages between connected users, sanitising incoming HTML before delegating to the domain ChatMessageManager.

## Public interface

Task<string> SendMessage(SendChatMessageInput input)
void Register()

## Used by

- [[signal-r-chat-communicator]] obtains IHubContext<ChatHub> to push events to specific connection IDs

## External dependencies

- Abp.AspNetCore.SignalR
- Castle.Windsor

## Notes

Extends OnlineClientHubBase. Uses IWindsorContainer.Release in Dispose to correctly return the hub to the Castle Windsor lifestyle manager. IAbpSession defaults to NullAbpSession.
