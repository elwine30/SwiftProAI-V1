---
title: "EfCoreOpenIddictAuthorizationRepository"
type: repository
language: csharp
layer: data
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.EntityFrameworkCore/OpenIddict/Authorizations/EfCoreOpenIddictAuthorizationRepository.cs"
updated: 2026-06-12
---

# EfCoreOpenIddictAuthorizationRepository

EF Core repository for OpenIddict authorisation grants, supporting multi-predicate find, subject lookup, application-scoped listing, and bulk pruning of stale grants.

## Public interface

Task<List<OpenIddictAuthorization>> FindAsync(string subject, Guid client, CancellationToken)
Task<List<OpenIddictAuthorization>> FindAsync(string, Guid, string, CancellationToken)
Task<List<OpenIddictAuthorization>> FindAsync(string, Guid, string, string, CancellationToken)
Task<List<OpenIddictAuthorization>> FindByApplicationIdAsync(Guid, CancellationToken)
Task<OpenIddictAuthorization> FindByIdAsync(Guid, CancellationToken)
Task<List<OpenIddictAuthorization>> FindBySubjectAsync(string, CancellationToken)
Task<List<OpenIddictAuthorization>> ListAsync(int?, int?, CancellationToken)
Task<long> PruneAsync(DateTime, CancellationToken)

## Dependencies

- [[thinkn-insur-tech-repository-base]] base repository providing EF Core UoW and DbContext access
- [[thinkn-insur-tech-db-context]] EF Core DbContext used to query OpenIddictAuthorization records
- [[ef-core-open-iddict-token-repository]] used during PruneAsync to cascade-delete associated tokens before removing authorisations

## Used by

- [[ef-core-open-iddict-token-repository]] references authorisation repository for cascaded operations

## External dependencies

- Abp.EntityFrameworkCore
- OpenIddict.Abstractions

## Notes

PruneAsync uses EF Core ExecuteDeleteAsync for server-side bulk deletes. ListAsync uses AsTracking(), which differs from the other OpenIddict repositories — may be intentional for the OpenIddict manager.
