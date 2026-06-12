---
title: "SignalRHelper"
type: class
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/shared/helpers/SignalRHelper.ts"
updated: 2026-06-12
---

# SignalRHelper

Bootstraps the ABP SignalR client by reading the encrypted auth token from local storage, configuring the abp.signalr global object and dynamically loading the signalr-client script.

## Public interface

- `static initSignalR(callback: () => void): void`
- `static updateQueryString(encryptedAuthToken: string): void`

## Dependencies

- [[app-consts]] provides the encrypted auth token cookie name constant
- [[local-storage-service]] retrieves the encrypted auth token from localForage before connecting

## Notes

Sets abp.signalr.autoConnect = false intentionally, deferring connection to ChatSignalrService which runs inside NgZone.runOutsideAngular. The signalr client JS is loaded dynamically as a script tag rather than being bundled — required because it depends on the global abp namespace.
