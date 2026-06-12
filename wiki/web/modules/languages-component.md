---
title: "LanguagesComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/admin/languages/languages.component.ts"
updated: 2026-06-12
---

# LanguagesComponent

Manages application localisation languages — listing, creating, deleting, setting the default, and navigating to the language-texts editor route.

## Public interface

getLanguages(): void
changeTexts(language: ApplicationLanguageListDto): void
setAsDefaultLanguage(language: ApplicationLanguageListDto): void
deleteLanguage(language: ApplicationLanguageListDto): void
get multiTenancySideIsHost(): boolean

## Dependencies

- [[create-or-edit-language-modal-component]] modal form for creating or editing a localisation language entry

## Used by

- [[create-or-edit-language-modal-component]]

## External dependencies

- @angular/core
- @angular/router
- abp-ng2-module
- primeng/table
- primeng/paginator
- rxjs/operators
