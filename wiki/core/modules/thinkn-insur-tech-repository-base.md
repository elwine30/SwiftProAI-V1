---
title: "ThinknInsurTechRepositoryBase"
type: class
language: csharp
layer: data
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.EntityFrameworkCore/EntityFrameworkCore/Repositories/ThinknInsurTechRepositoryBase.cs"
updated: 2026-06-12
---

# ThinknInsurTechRepositoryBase

Abstract base repository that wires the ABP EfCore repository to ThinknInsurTechDbContext, providing a typed generic and a convenience int-keyed variant for all custom repositories to inherit from.

## Public interface

ThinknInsurTechRepositoryBase<TEntity, TPrimaryKey>(IDbContextProvider<ThinknInsurTechDbContext>)
ThinknInsurTechRepositoryBase<TEntity>(IDbContextProvider<ThinknInsurTechDbContext>)

## Dependencies

- [[thinkn-insur-tech-db-context]] DbContext binding for all repository queries

## Used by

- [[thinkn-insur-tech-db-context]]
- [[user-repository]]
- [[subscription-payment-repository]]
- [[user-organization-unit-repository]]
- [[ef-core-open-iddict-application-repository]]
- [[ef-core-open-iddict-authorization-repository]]
- [[ef-core-open-iddict-scope-repository]]
- [[ef-core-open-iddict-token-repository]]

## External dependencies

- Abp.EntityFrameworkCore

## Notes

No custom methods are added here — the class exists solely to bind the generic EfCoreRepositoryBase to the project's DbContext type. The int-key variant inherits from the generic variant and intentionally adds no members.
