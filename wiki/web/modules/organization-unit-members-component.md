---
title: "OrganizationUnitMembersComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/admin/organization-units/organization-unit-members.component.ts"
updated: 2026-06-12
---

# OrganizationUnitMembersComponent

Detail panel listing users assigned to the currently selected organisation unit, with lazy-load pagination and add/remove member operations that emit events back to the tree to update counts.

## Public interface

@Output() memberRemoved: EventEmitter<IUserWithOrganizationUnit>
@Output() membersAdded: EventEmitter<IUsersWithOrganizationUnit>
set organizationUnit(ou: IBasicOrganizationUnitInfo)
getOrganizationUnitUsers(event?: LazyLoadEvent): void

## Dependencies

- [[add-member-modal-component]] modal for searching and adding users to the selected OU

## Used by

- [[organization-units-component]]
- [[add-member-modal-component]]

## External dependencies

- @angular/core
- primeng/api
- primeng/table
- primeng/paginator
- rxjs/operators
