---
title: "CaseLawyer"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Registration/CaseLawyer.cs"
updated: 2026-06-12
---

# CaseLawyer

Join entity linking a claim registration to a law firm, with case-level contact and hearing details.

## Public interface

- `DateTime HearingDate`
- `string ReferenceNo` / `string ContactNo` / `string ContactName` / `string Email`
- `string Type` — e.g. plaintiff or defence
- `int RegisterId` / `MainRegistration RegisterFk`
- `int LawFirmId` / `LawFirm LawFirmFk`

## Dependencies

- [[main-registration]] parent registration FK
- [[law-firm]] referenced law firm

## External dependencies

- Abp.Zero (FullAuditedEntity, IMayHaveTenant, IMayHaveOrganizationUnit)
