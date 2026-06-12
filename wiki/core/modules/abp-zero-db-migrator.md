---
title: "AbpZeroDbMigrator"
type: class
language: csharp
layer: data
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.EntityFrameworkCore/EntityFrameworkCore/AbpZeroDbMigrator.cs"
updated: 2026-06-12
---

# AbpZeroDbMigrator

Thin subclass of the ABP Zero migrator that binds it to ThinknInsurTechDbContext, enabling per-tenant connection migration support.

## Public interface

AbpZeroDbMigrator(IUnitOfWorkManager, IDbPerTenantConnectionStringResolver, IDbContextResolver)

## Dependencies

- [[thinkn-insur-tech-db-context]] bound as the generic type parameter for the ABP migrator

## Used by

- [[thinkn-insur-tech-entity-framework-core-module]]

## External dependencies

- Abp.Zero.EntityFrameworkCore

## Notes

No logic beyond constructor wiring; all behaviour is inherited from AbpZeroDbMigrator<T>.
