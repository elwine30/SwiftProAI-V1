---
title: "ChatBarComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/shared/layout/chat/chat-bar.component.ts"
updated: 2026-06-12
---

# ChatBarComponent

Full chat panel slide-out drawer allowing friend selection, message history loading, image/file upload and real-time message receipt via SignalR events.

## Public interface

- `@Output() onProgress: EventEmitter<any>`
- `@Input() addFriendModal: AddFriendModalComponent`
- `isOpen: boolean (getter/setter persisted to LocalForage)`
- `pinned: boolean (getter/setter persisted to LocalForage)`
- `selectedUser: ChatFriendDto (getter/setter)`
- `sendMessage(event?): void`
- `selectFriend(friend): void`
- `loadMessages(user, callback): void`
- `markAllUnreadMessagesOfUserAsRead(user): void`
- `getFilteredFriends(state, filter): FriendDto[]`

## Dependencies

- [[chat-signalr-service]] manages the SignalR WebSocket connection for real-time messaging
- [[chat-friend-dto]] data shape for friend entries in the chat list
- [[date-time-service]] formats message timestamps in the chat panel

## External dependencies

- `@angular/core`
- `@angular/common/http`
- `primeng/fileupload`
- `lodash-es`
- `luxon`
- `rxjs`

## Notes

Uses Metronic DrawerComponent JS directly for panel open/close events. Chat state (open/pinned/selected user) is persisted to LocalForage via LocalStorageService. Dispatches unreadMessageCount changes via abp.event.trigger.
