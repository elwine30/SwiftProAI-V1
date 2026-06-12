---
title: "ThinknInsurTechDbContextFactory"
type: class
language: csharp
layer: data
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.EntityFrameworkCore/EntityFrameworkCore/ThinknInsurTechDbContextFactory.cs"
updated: 2026-06-12
---

# ThinknInsurTechDbContextFactory

Design-time factory used exclusively by the EF Core CLI tooling to create a DbContext instance when running migrations outside the application host.

## Public interface

ThinknInsurTechDbContext CreateDbContext(string[] args)

## Dependencies

- [[thinkn-insur-tech-db-context]] the context being instantiated
- [[thinkn-insur-tech-db-context-configurer]] applies provider options to the builder

## External dependencies

- Microsoft.EntityFrameworkCore.Design

## Notes

Not used at runtime. Relies on AppConfigurations and WebContentDirectoryFinder to locate appsettings.json from the file system, supporting user secrets for development.
