---
title: "LocalStorageService"
type: service
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/shared/utils/local-storage.service.ts"
updated: 2026-06-12
---

# LocalStorageService

Wraps localForage to provide async key-value persistent storage with callback-based API for storing encrypted auth tokens and other client-side data.

## Public interface

- `getItem(key: string, callback: any): void`
- `setItem(key, value, callback?): void`
- `removeItem(key, callback?): void`

## Used by

- [[signalr-helper]]
- [[utils-module]]

## External dependencies

- `localforage`

## Notes

Nulls are stored as undefined to avoid localForage null-serialisation quirks. Used by SignalRHelper and by the auth layer to retrieve the encrypted auth token.
