---
title: "TreeDataHelperService"
type: service
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/shared/utils/tree-data-helper.service.ts"
updated: 2026-06-12
---

# TreeDataHelperService

Provides recursive search and traversal helpers for PrimeNG-style tree node structures.

## Public interface

- `findNode(data, selector): any`
- `findParent(data, nodeSelector): any`
- `findChildren(data, selector): string[]`

## Used by

- [[utils-module]]

## External dependencies

- `@angular/core`
- `lodash-es`
