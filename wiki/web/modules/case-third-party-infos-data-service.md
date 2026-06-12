---
title: "CaseThirdPartyInfosDataService"
type: service
language: typescript
layer: application
path: "SwiftProAI.Web/angular/src/app/main/registration/caseThirdPartyInfos/caseThirdPartyInfosDataService.ts"
updated: 2026-06-12
---

# CaseThirdPartyInfosDataService

Coordinates selection and refresh events between the third-party info list and edit form using two BehaviorSubject streams.

## Public interface

selectedItem$: Observable<number>
refreshPage$: Observable<void>
selectItem(itemId: number): void
refreshPage(): void

## Used by

- [[case-third-party-infos-component]]

## External dependencies

- @angular/core
- rxjs

## Notes

Same pattern as CaseClaimDataService. The refreshPage$ subject emits void — subscribers call reloadPage() on receive.
