---
title: "EditionsComponent"
type: component
language: typescript
layer: presentation
path: "SwiftProAI.Web/angular/src/app/admin/editions/editions.component.ts"
updated: 2026-06-12
---

# EditionsComponent

Host-only page for managing subscription editions (SaaS tiers), supporting create, edit, delete, and tenant-migration operations between editions.

## Public interface

getEditions(): void
createEdition(): void
deleteEdition(edition: EditionListDto): void

## Dependencies

- [[create-edition-modal-component]] modal for creating a new subscription edition
- [[edit-edition-modal-component]] modal for editing an existing edition's features and pricing
- [[move-tenants-to-another-edition-modal-component]] modal for bulk-migrating tenants from one edition to another

## Used by

- [[create-edition-modal-component]]
- [[edit-edition-modal-component]]
- [[move-tenants-to-another-edition-modal-component]]

## External dependencies

- @angular/core
- primeng/table
- primeng/paginator
- rxjs/operators
