---
title: "CaseThirdPartyInfosComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/main/registration/caseThirdPartyInfos/caseThirdPartyInfos.component.ts"
updated: 2026-06-12
---

# CaseThirdPartyInfosComponent

List of third-party persons for a case, coordinating selection events with sibling edit form via CaseThirdPartyInfosDataService and scrolling to top on selection.

## Public interface

getCaseThirdPartyInfos(event?): void
selectItem(item): void — emits selection to data service and scrolls to top
deleteCaseThirdPartyInfo(info): void
reloadPage(): void

## Dependencies

- [[case-third-party-infos-data-service]] event bus for coordinating selection with sibling edit form

## Used by

- [[case-third-party-infos-component]]

## External dependencies

- @angular/core
- @angular/router
- primeng/api
- primeng/table
- primeng/paginator
- luxon
- lodash-es

## Notes

Subscribes to dataService.refreshPage$ on init to reload when sibling edit form saves. Uses AppConsts.isComponentDisabled global flag to hide actions in read-only mode.
