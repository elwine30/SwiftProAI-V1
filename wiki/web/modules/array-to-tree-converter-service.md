---
title: "ArrayToTreeConverterService"
type: service
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/shared/utils/array-to-tree-converter.service.ts"
updated: 2026-06-12
---

# ArrayToTreeConverterService

Converts a flat array with parent-ID references into a nested tree structure compatible with PrimeNG tree components.

## Public interface

- `createTree(array, parentIdProperty, idProperty, parentIdValue, childrenProperty, fieldMappings): any`
- `mapFields(node, newNode, fieldMappings): void`

## Used by

- [[utils-module]]

## External dependencies

- `@angular/core`
- `lodash-es`
