---
title: "EfCoreOpenIddictTokenRepository"
type: repository
language: csharp
layer: data
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.EntityFrameworkCore/OpenIddict/Tokens/EfCoreOpenIddictTokenRepository.cs"
updated: 2026-06-12
---

# EfCoreOpenIddictTokenRepository

EF Core repository for OpenIddict tokens, supporting multi-predicate lookup, bulk deletion by application or authorization ID, pruning of expired tokens, and server-side revocation.

## Public interface

Task DeleteManyByApplicationIdAsync(Guid, bool, CancellationToken)
Task DeleteManyByAuthorizationIdAsync(Guid, bool, CancellationToken)
Task DeleteManyByAuthorizationIdsAsync(Guid[], bool, CancellationToken)
Task<List<OpenIddictToken>> FindAsync(string, Guid, CancellationToken)
Task<OpenIddictToken> FindByIdAsync(Guid, CancellationToken)
Task<OpenIddictToken> FindByReferenceIdAsync(string, CancellationToken)
Task<List<OpenIddictToken>> FindBySubjectAsync(string, CancellationToken)
Task<long> PruneAsync(DateTime, CancellationToken)
ValueTask<long> RevokeByAuthorizationIdAsync(Guid, CancellationToken)

## Dependencies

- [[thinkn-insur-tech-repository-base]] base repository providing EF Core UoW and DbContext access
- [[thinkn-insur-tech-db-context]] EF Core DbContext used to query and mutate OpenIddictToken records
- [[ef-core-open-iddict-authorization-repository]] referenced for cascaded revocation and pruning workflows

## Used by

- [[ef-core-open-iddict-authorization-repository]] depends on token repository for cascaded delete during prune

## External dependencies

- Abp.EntityFrameworkCore
- OpenIddict.Abstractions
- Z.EntityFramework.Plus

## Notes

Bulk deletes use Z.EntityFramework.Plus DeleteAsync for application-ID and authorization-ID variants. PruneAsync and RevokeByAuthorizationIdAsync use EF Core ExecuteDeleteAsync/ExecuteUpdateAsync for server-side operations. RevokeByAuthorizationIdAsync returns ValueTask<long>.
