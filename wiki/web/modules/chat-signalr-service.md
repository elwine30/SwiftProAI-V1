---
title: "ChatSignalrService"
type: service
language: typescript
layer: application
path: "SwiftProAI.Web/angular/src/app/shared/layout/chat/chat-signalr.service.ts"
updated: 2026-06-12
---

# ChatSignalrService

Manages the SignalR WebSocket connection for real-time chat, handles exponential-backoff reconnection and bridges incoming hub events to ABP's event bus.

## Public interface

- `chatHub: HubConnection`
- `isChatConnected: boolean`
- `configureConnection(connection): void`
- `registerChatEvents(connection): void`
- `sendMessage(messageData, callback): void`
- `init(): void`

## Used by

- [[chat-bar-component]]

## External dependencies

- `@angular/core`
- `@microsoft/signalr`

## Notes

Runs connection setup outside Angular zone (NgZone.runOutsideAngular) to avoid triggering unnecessary change detection. All incoming hub events are re-published via abp.event.trigger rather than RxJS subjects.
