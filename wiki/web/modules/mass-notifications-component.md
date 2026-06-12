---
title: "MassNotificationsComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/admin/mass-notifications/mass-notifications.component.ts"
updated: 2026-06-12
---

# MassNotificationsComponent

Admin page for viewing sent mass notifications with date range filter and inline message preview, detecting HTML content to render safely, and launching the create notification modal.

## Public interface

getPublishedNotifications(event?: LazyLoadEvent): void
createMassNotification(): void
reloadPage(): void
showMessageDetailModal(str: string): void
isHTMLMessage(str): boolean
getSeverityClass(severity: number): string

## Dependencies

- [[create-mass-notification-modal-component]] modal form for composing and publishing a new mass notification

## Used by

- [[create-mass-notification-modal-component]]

## External dependencies

- @angular/core
- ngx-bootstrap/modal
- primeng/api
- primeng/table
- primeng/paginator
- luxon
