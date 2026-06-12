---
title: "OrganizationTreeComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/admin/organization-units/organization-tree.component.ts"
updated: 2026-06-12
---

# OrganizationTreeComponent

Interactive PrimeNG tree of all organisation units with drag-and-drop node re-parenting, context-menu CRUD operations, and real-time member/role count updates without a full reload.

## Public interface

@Output() ouSelected: EventEmitter<IBasicOrganizationUnitInfo>
nodeSelect(event): void
nodeDrop(event): void
addUnit(parentId?: number): void
unitCreated(ou: OrganizationUnitDto): void
unitUpdated(ou: OrganizationUnitDto): void
deleteUnit(id): void
membersAdded(data: IUsersWithOrganizationUnit): void
memberRemoved(data: IUserWithOrganizationUnit): void
reload(): void

## Dependencies

- [[create-or-edit-unit-modal-component]] modal for creating or renaming an organisation unit node

## Used by

- [[organization-units-component]]
- [[create-or-edit-unit-modal-component]]

## External dependencies

- @angular/core
- primeng/api
- rxjs
- lodash-es

## Notes

Uses ArrayToTreeConverterService and TreeDataHelperService from src/shared/utils to convert flat OU list to PrimeNG TreeNode hierarchy. Entity history is conditionally appended to the context menu based on `(abp as any).custom.EntityHistory`.
