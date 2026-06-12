---
title: "OrganizationUnitsComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/admin/organization-units/organization-units.component.ts"
updated: 2026-06-12
---

# OrganizationUnitsComponent

Container component that coordinates the OU tree, members panel, and roles panel, propagating the selected tree node to the two detail panels via property binding.

## Public interface

ouSelected(event: any): void

## Dependencies

- [[organization-tree-component]] interactive PrimeNG OU tree with drag-and-drop re-parenting
- [[organization-unit-members-component]] detail panel listing users assigned to the selected OU
- [[organization-unit-roles-component]] detail panel listing roles assigned to the selected OU

## Used by

- [[organization-tree-component]]
- [[organization-unit-members-component]]
- [[organization-unit-roles-component]]

## External dependencies

- @angular/core
