---
title: "CaseExpense"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Registration/CaseExpense.cs"
updated: 2026-06-12
---

# CaseExpense

Entity tracking an individual out-of-pocket expense submitted by an adjuster on a case, with approval state and lookup-driven type classification.

## Public interface

- `double Amount` / `double ApprovedAmount`
- `string Remark`
- `bool Approved` / `bool Rejected`
- `int StatusId` / `Lookup StatusFk`
- `int? TypeId` / `Lookup TypeFk`
- `int? SubTypeId` / `Lookup SubTypeFk`
- `int RegisterId` / `MainRegistration RegisterFk`
- `long AdjusterId` / `User AdjusterFk`

## Dependencies

- [[main-registration]] parent registration FK
- [[lookup]] status, type and sub-type FKs

## External dependencies

- Abp.Zero (FullAuditedEntity, IMustHaveTenant, IMayHaveOrganizationUnit)

## Notes

- Expense type and sub-type are driven by the `Lookup` table rather than enums — new types can be added without code changes.
