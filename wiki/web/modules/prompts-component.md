---
title: "PromptsComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/admin/ocr/prompts/prompts.component.ts"
updated: 2026-06-12
---

# PromptsComponent

CRUD management page for AI OCR prompt templates (prompt name and request text), used to configure the prompts sent to the OpenAI API for document extraction.

## Public interface

getPrompts(event?: LazyLoadEvent): void
reloadPage(): void
resetFilters(): void

## Dependencies

- [[create-or-edit-prompt-modal-component]] modal form for creating or editing a prompt template
- [[view-prompt-modal-component]] read-only view modal for inspecting a prompt template

## Used by

- [[create-or-edit-prompt-modal-component]]
- [[view-prompt-modal-component]]

## External dependencies

- @angular/core
- @angular/router
- abp-ng2-module
- primeng/table
- primeng/paginator
- primeng/api
- luxon
