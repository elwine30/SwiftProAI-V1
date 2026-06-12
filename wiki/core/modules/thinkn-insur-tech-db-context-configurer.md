---
title: "ThinknInsurTechDbContextConfigurer"
type: class
language: csharp
layer: data
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.EntityFrameworkCore/EntityFrameworkCore/ThinknInsurTechDbContextConfigurer.cs"
updated: 2026-06-12
---

# ThinknInsurTechDbContextConfigurer

Static helper that applies SQL Server provider configuration to a DbContextOptionsBuilder, accepting either a connection string or an existing DbConnection.

## Public interface

static void Configure(DbContextOptionsBuilder<ThinknInsurTechDbContext>, string connectionString)
static void Configure(DbContextOptionsBuilder<ThinknInsurTechDbContext>, DbConnection connection)

## Dependencies

- [[thinkn-insur-tech-db-context]] the context type being configured

## Used by

- [[thinkn-insur-tech-entity-framework-core-module]]
- [[thinkn-insur-tech-db-context-factory]]

## External dependencies

- Microsoft.EntityFrameworkCore.SqlServer

## Notes

Commented-out UseNpgsql calls remain from the pre-migration PostgreSQL provider. Optional SQL logging via a LoggerFactory is also commented out.
