---
title: "CaseClaimDataService"
type: service
language: typescript
layer: application
path: "SwiftProAI.Web/angular/src/app/main/registration/caseClaims/caseClaimDataService.ts"
updated: 2026-06-12
---

# CaseClaimDataService

Singleton cross-component state service that broadcasts the current claim status ID to any subscriber via a BehaviorSubject.

## Public interface

currentStatusId: Observable<number|null>
changeStatusId(statusId: number|null): void

## Used by

- [[create-or-edit-case-claim-component]]

## External dependencies

- @angular/core
- rxjs

## Notes

Provided in root; acts as a lightweight in-memory event bus between CreateOrEditCaseClaimComponent and its siblings. No persistence.
