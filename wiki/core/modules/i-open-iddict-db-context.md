---
title: "IOpenIddictDbContext"
type: interface
language: csharp
layer: data
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.EntityFrameworkCore/EntityFrameworkCore/IOpenIddictDbContext.cs"
updated: 2026-06-12
---

# IOpenIddictDbContext

Contract that any DbContext must satisfy to support OpenIddict, exposing DbSets for Applications, Authorizations, Scopes and Tokens.

## Public interface

DbSet<OpenIddictApplication> Applications { get; }
DbSet<OpenIddictAuthorization> Authorizations { get; }
DbSet<OpenIddictScope> Scopes { get; }
DbSet<OpenIddictToken> Tokens { get; }

## Used by

- [[thinkn-insur-tech-db-context]]
- [[ef-core-open-iddict-application-repository]]

## External dependencies

- Microsoft.EntityFrameworkCore

## Notes

Implemented by ThinknInsurTechDbContext. Enables OpenIddict repositories to work against an abstraction rather than the concrete context.
