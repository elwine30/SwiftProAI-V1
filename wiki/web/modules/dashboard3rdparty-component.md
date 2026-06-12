---
title: "Dashboard3rdPartyComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/main/dashboard3rdParty/dashboard3rdParty.component.ts"
updated: 2026-06-12
---

# Dashboard3rdPartyComponent

Variant dashboard for third-party adjusters using a reduced status set (UnderInvestigation, Completed, Cancelled) and an additional adjuster-company filter.

## Public interface

getMainRegistrationDetails(event?): void
getDashboardSummary(): void
onClick(statusId): void
onRowClicked(id, isView): void
exportTable(): void
filterAdjuster(): void — reloads adjuster dropdown filtered by selected adjuster company OU

## External dependencies

- @angular/core
- @angular/router
- @angular/common
- primeng/api
- primeng/table
- primeng/paginator
- rxjs/operators
- luxon
- lodash-es

## Notes

Nearly identical to DashboardComponent but uses Enum3rdPartyRegistrationStatus and adds an adjuster company (OU) filter dropdown. Shares the same MainRegistrationServiceProxy endpoint.
