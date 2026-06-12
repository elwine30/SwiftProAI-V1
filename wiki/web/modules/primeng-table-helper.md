---
title: "PrimengTableHelper"
type: class
language: typescript
layer: shared
path: "SwiftProAI.Web/angular/src/shared/helpers/PrimengTableHelper.ts"
updated: 2026-06-12
---

# PrimengTableHelper

Provides shared state and utility methods for PrimeNG p-table instances, managing loading state, pagination and sort-string construction.

## Public interface

- `records: any[]`
- `totalRecordsCount: number`
- `isLoading: boolean`
- `showLoadingIndicator(): void`
- `hideLoadingIndicator(): void`
- `getSorting(table: Table): string`
- `getMaxResultCount(paginator, event): number`
- `getSkipCount(paginator, event): number`
- `shouldResetPaging(event): boolean`
- `adjustScroll(table: Table): void`

## Used by

- [[app-component-base]]

## External dependencies

- `primeng/api`
- `primeng/paginator`
- `primeng/table`
- `rtl-detect`

## Notes

Instantiated directly with new PrimengTableHelper() inside AppComponentBase (not via DI). adjustScroll() uses abp.localization.currentLanguage.name to detect RTL and syncs scroll position between the header and body elements.
