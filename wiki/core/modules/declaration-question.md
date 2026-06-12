---
title: "DeclarationQuestion"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Registration/DeclarationQuestion.cs"
updated: 2026-06-12
---

# DeclarationQuestion

Entity defining a configurable declaration question presented to claimants during the registration workflow.

## Public interface

- `string Question` (required)
- `string OptionType` — controls the UI input type (e.g. radio, checkbox, text)
- `string OptionValues` — pipe-delimited or JSON-encoded answer options for multi-choice questions

## Dependencies

(none within this project — referenced by `CaseDeclarationAnswer`)

## External dependencies

- Abp.Zero (FullAuditedEntity, IMustHaveTenant, IMayHaveOrganizationUnit)
