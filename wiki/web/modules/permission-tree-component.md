---
title: "PermissionTreeComponent"
type: component
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/app/admin/shared/permission-tree.component.ts"
updated: 2026-06-12
---

# PermissionTreeComponent

Reusable PrimeNG tree that renders the full permission hierarchy, supports single or multi-select with optional cascade (auto-select parents/deselect children), and exposes the selected permission names as strings.

## Public interface

@Input() singleSelect: boolean
@Input() disableCascade: boolean
set editData(val: PermissionTreeEditModel)
getGrantedPermissionNames(): string[]
nodeSelect(event): void
onNodeUnselect(event): void
filterPermissions(event): void

## Used by

- [[create-or-edit-role-modal-component]]
- [[permission-tree-modal-component]]

## External dependencies

- @angular/core
- primeng/api
- lodash-es

## Notes

Uses ArrayToTreeConverterService and TreeDataHelperService from src/shared/utils. The filter applies a 'hidden-tree-node' CSS class rather than splicing the data array.
