---
title: "UserQuery"
type: class
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.GraphQL/Queries/UserQuery.cs"
updated: 2026-06-12
---

# UserQuery

GraphQL query resolver that returns a paginated, filtered list of users with optionally included roles and organisation units, guarded by the Pages_Administration_Users permission.

## Public interface

- `override Task<PagedResultDto<UserDto>> Resolve(IResolveFieldContext context)`

## Dependencies

- [[thinkninsurtech-query-base]] abstract base providing unit-of-work pipeline and AutoMapper projection helpers
- [[context-extensions]] fluent argument handling and HasSelectionField for conditional EF includes
- [[graphql-user-dto]] DTO shape returned to the API caller
- [[user-paged-result-graph-type]] GraphQL type wrapping the paginated result
- [[user-type]] provides ChildFields path constants used in HasSelectionField checks

## Used by

- [[query-container]]

## External dependencies

- Abp.Authorization
- Abp.Domain.Repositories
- System.Linq.Dynamic.Core
- Microsoft.EntityFrameworkCore

## Notes

Uses HasSelectionField to conditionally Include Roles and OrganizationUnits EF navigation properties only when those fields were requested in the GraphQL query — a manual N+1 guard. A TODO exists to reduce organisation unit fetching from two queries to one. Pagination defaults to AppConsts.DefaultPageSize.
