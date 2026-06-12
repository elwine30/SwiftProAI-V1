---
title: "RoleQuery"
type: class
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.GraphQL/Queries/RoleQuery.cs"
updated: 2026-06-12
---

# RoleQuery

GraphQL query resolver that retrieves a filtered list of roles via RoleManager, requiring the Pages_Administration_Roles permission.

## Public interface

- `override Task<List<RoleDto>> Resolve(IResolveFieldContext context)`

## Dependencies

- [[thinkninsurtech-query-base]] abstract base providing unit-of-work pipeline and AutoMapper projection helpers
- [[context-extensions]] fluent argument handling for filter composition
- [[graphql-role-dto]] DTO shape returned to the API caller
- [[role-type]] GraphQL type definition wired into the field type

## Used by

- [[query-container]]

## External dependencies

- Abp.Authorization
- GraphQL
- Microsoft.EntityFrameworkCore

## Notes

Filters by id or name. Delegates to RoleManager.Roles IQueryable rather than a raw repository.
