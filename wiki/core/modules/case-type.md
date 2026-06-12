---
title: "CaseType"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Case/CaseType.cs"
updated: 2026-06-12
---

# CaseType

Reference entity classifying the type of insurance claim case (e.g. motor, fire, marine).

## Public interface

- `string Description`
- `string ShortName`
- `bool IsActive`

## External dependencies

- Abp.Zero (FullAuditedEntity)

## Notes

- Used as a foreign key by `MainRegistration`, `InsuranceCompany`, `CaseInvoice`, `CaseDebitNote` and `CaseCreditNote`.
- `CaseTypeManager` provides CRUD operations; `ICaseTypeManager` is the DI interface.
