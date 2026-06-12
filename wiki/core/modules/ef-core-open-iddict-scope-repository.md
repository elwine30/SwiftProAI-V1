---
title: "EfCoreOpenIddictScopeRepository"
type: repository
language: csharp
layer: data
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.EntityFrameworkCore/OpenIddict/Scopes/EfCoreOpenIddictScopeRepository.cs"
updated: 2026-06-12
---

# EfCoreOpenIddictScopeRepository

EF Core repository for OpenIddict scopes, supporting lookup by id, name, multiple names, and associated resource, as well as paginated listing with optional text filter.

## Public interface

Task<List<OpenIddictScope>> GetListAsync(string, int, int, string, CancellationToken)
Task<long> GetCountAsync(string, CancellationToken)
Task<OpenIddictScope> FindByIdAsync(Guid, CancellationToken)
Task<OpenIddictScope> FindByNameAsync(string, CancellationToken)
Task<List<OpenIddictScope>> FindByNamesAsync(string[], CancellationToken)
Task<List<OpenIddictScope>> FindByResourceAsync(string, CancellationToken)
Task<List<OpenIddictScope>> ListAsync(int?, int?, CancellationToken)

## Dependencies

- [[thinkn-insur-tech-repository-base]] base repository providing EF Core UoW and DbContext access
- [[thinkn-insur-tech-db-context]] EF Core DbContext used to query OpenIddictScope records

## External dependencies

- Abp.EntityFrameworkCore
- System.Linq.Dynamic.Core

## Notes

Filter in GetListAsync and GetCountAsync matches against Name, DisplayName, and Description fields. All queries wrapped in WithUnitOfWorkAsync.
