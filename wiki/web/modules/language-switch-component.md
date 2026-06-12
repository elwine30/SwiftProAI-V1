---
title: "LanguageSwitchComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/account/language-switch.component.ts"
updated: 2026-06-12
---

# LanguageSwitchComponent

Renders a language dropdown on account pages, persisting the user's selection in a long-lived ABP culture cookie and reloading the page.

## Public interface

changeLanguage(language: abp.localization.ILanguageInfo): void
currentLanguage: abp.localization.ILanguageInfo
languages: abp.localization.ILanguageInfo[]

## Used by

- [[account-module]]

## External dependencies

- @angular/core
- lodash-es

## Notes

Reads language list directly from the abp.localization global, filtering out disabled languages.
