---
title: "CaseAdjustersComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/main/registration/caseAdjusters/caseAdjusters.component.ts"
updated: 2026-06-12
---

# CaseAdjustersComponent

Paginated list of case adjuster assignments with text filters; delete is commented out, navigation goes to createOrEdit page.

## Public interface

getCaseAdjusters(event?): void
createCaseAdjuster(): void
reloadPage(): void
resetFilters(): void

## External dependencies

- @angular/core
- @angular/router
- primeng/api
- primeng/table
- primeng/paginator
- abp-ng2-module
- luxon
- lodash-es

## Notes

Delete method is fully commented out. This is a standard ABP-scaffolded list component with minimal customisation.
