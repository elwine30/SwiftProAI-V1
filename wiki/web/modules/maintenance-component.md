---
title: "MaintenanceComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/admin/maintenance/maintenance.component.ts"
updated: 2026-06-12
---

# MaintenanceComponent

System maintenance panel exposing server cache management (list, clear individual, clear all), live web log viewer with log-level colour coding, log download, and new-version notification broadcast.

## Public interface

getCaches(): void
clearCache(cacheName): void
clearAllCaches(): void
getWebLogs(): void
downloadWebLogs(): void
sendNewVersionAvailableNotification(): void
getLogClass(log: string): string
getLogType(log: string): string

## External dependencies

- @angular/core
- rxjs/operators
- lodash-es

## Notes

Uses `abp.message.confirm` from the global namespace rather than the typed helper. Log panel height is adjusted with direct DOM manipulation via document.getElementsByClassName.
