---
title: "FriendProfilePictureComponent"
type: component
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/shared/utils/friend-profile-picture.component.ts"
updated: 2026-06-12
---

# FriendProfilePictureComponent

Renders a user's profile picture by fetching it as base64 from the ProfileServiceProxy, falling back to a default image.

## Public interface

- `@Input() userId: number`
- `@Input() tenantId: number`

## Dependencies

- [[app-consts]] — default image path and application constants

## Used by

- [[utils-module]]

## External dependencies

- `@angular/core`

## Notes

Used in chat and notification components to display friend avatars. Fetches on AfterViewInit rather than OnInit to ensure the DOM is ready.
