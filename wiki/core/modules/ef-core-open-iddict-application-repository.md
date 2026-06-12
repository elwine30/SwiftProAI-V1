---
title: "EfCoreOpenIddictApplicationRepository"
type: repository
language: csharp
layer: data
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.EntityFrameworkCore/OpenIddict/Applications/EfCoreOpenIddictApplicationRepository.cs"
updated: 2026-06-12
---

# EfCoreOpenIddictApplicationRepository

EF Core repository for OpenIddict OAuth2 client applications, supporting lookup by client ID, redirect URIs, post-logout URIs, and paginated listing.

## Public interface

Task<List<OpenIddictApplication>> GetListAsync(string, int, int, string, CancellationToken)
Task<long> GetCountAsync(string, CancellationToken)
Task<OpenIddictApplication> FindByClientIdAsync(string, CancellationToken)
Task<List<OpenIddictApplication>> FindByPostLogoutRedirectUriAsync(string, CancellationToken)
Task<List<OpenIddictApplication>> FindByRedirectUriAsync(string, CancellationToken)
Task<List<OpenIddictApplication>> ListAsync(int?, int?, CancellationToken)

## Dependencies

- [[thinkn-insur-tech-repository-base]] base repository providing EF Core UoW and DbContext access
- [[thinkn-insur-tech-db-context]] EF Core DbContext used to query OpenIddictApplication records
- [[i-open-iddict-db-context]] interface abstraction for the OpenIddict-aware DbContext

## Used by

- [[thinkn-insur-tech-db-context]] registered as a dependency of the DbContext configuration
- [[ef-core-open-iddict-authorization-repository]] indirectly via shared DbContext

## External dependencies

- Abp.EntityFrameworkCore
- System.Linq.Dynamic.Core

## Notes

All queries are wrapped in WithUnitOfWorkAsync to ensure they execute within a unit of work scope, as required by ABP's UoW management. Dynamic sorting is applied via System.Linq.Dynamic.Core.
