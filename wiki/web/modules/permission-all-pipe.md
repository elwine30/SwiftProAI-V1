---
title: "PermissionAllPipe"
type: class
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/shared/common/pipes/permission-all.pipe.ts"
updated: 2026-06-12
---

# PermissionAllPipe

Angular pipe that returns true only when the current user holds every one of the supplied permission names.

## Public interface

- `transform(arrPermissions: string[]): boolean`

## Used by

- [[utils-module]]

## External dependencies

- `@angular/core`
- `abp-ng2-module`
