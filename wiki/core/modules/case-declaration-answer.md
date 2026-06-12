---
title: "CaseDeclarationAnswer"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Registration/CaseDeclarationAnswer.cs"
updated: 2026-06-12
---

# CaseDeclarationAnswer

Entity recording a claimant's answer to a specific declaration question for a case registration.

## Public interface

- `string Answer`
- `int RegisterId` / `MainRegistration RegisterFk`
- `int QuestionId` / `DeclarationQuestion QuestionFk`

## Dependencies

- [[main-registration]] parent registration FK
- [[declaration-question]] question definition FK

## External dependencies

- Abp.Zero (FullAuditedEntity, IMustHaveTenant, IMayHaveOrganizationUnit)
