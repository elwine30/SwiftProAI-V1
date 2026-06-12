---
title: "OpenIddictDbContextModelCreatingExtensions"
type: class
language: csharp
layer: data
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.EntityFrameworkCore/EntityFrameworkCore/OpenIddictDbContextModelCreatingExtensions.cs"
updated: 2026-06-12
---

# OpenIddictDbContextModelCreatingExtensions

Extension method that applies OpenIddict entity configurations (table names, indexes, max-length constraints, and foreign keys) to the ModelBuilder during OnModelCreating.

## Public interface

static void ConfigureOpenIddict(this ModelBuilder builder)

## External dependencies

- Microsoft.EntityFrameworkCore

## Notes

Also defines the companion consts classes (OpenIddictApplicationConsts, OpenIddictAuthorizationConsts, OpenIddictScopeConsts, OpenIddictTokenConsts) in the same file. All tables are prefixed 'OpenIddict' with no schema.
