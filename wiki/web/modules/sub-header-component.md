---
title: "SubHeaderComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/shared/common/sub-header/sub-header.component.ts"
updated: 2026-06-12
---

# SubHeaderComponent

Reusable page sub-header that renders a title, optional description and a breadcrumb trail with navigable links.

## Public interface

- `@Input() title: string`
- `@Input() description: string`
- `@Input() breadcrumbs: BreadcrumbItem[]`
- `goToBreadcrumb(breadcrumb: BreadcrumbItem): void`

## External dependencies

- `@angular/core`
- `@angular/router`

## Notes

BreadcrumbItem class is declared in the same file and supports optional NavigationExtras for parameterised breadcrumb navigation.
