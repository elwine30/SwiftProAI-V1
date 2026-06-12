---
title: "ThirdPartyViewComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/admin/common/thirdPartyViewApproval/thirdPartyViewApproval.component.ts"
updated: 2026-06-12
---

# ThirdPartyViewComponent

Tabbed approval page for third-party case-view access requests, delegating data loading to two child components (onboarded and not-yet-onboarded data tables) and filtering them by status.

## Public interface

getDataTable(): void
getDataCountNotOnboarded(): number
getDataCountOnboard(): number
onChangeStatus(event: number): void
onTabChange(tab: string): void

## Dependencies

- [[not-onboard-data-table-component]] child table for third-party users not yet onboarded
- [[on-board-data-table-component]] child table for third-party users already onboarded

## Used by

- [[not-onboard-data-table-component]]
- [[on-board-data-table-component]]

## External dependencies

- @angular/core
- ngx-bootstrap/tabs
- primeng/api
- primeng/table
- primeng/paginator
- luxon
- rxjs
