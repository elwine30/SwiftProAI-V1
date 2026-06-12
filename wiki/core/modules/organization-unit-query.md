---
title: "OrganizationUnitQuery"
type: class
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.GraphQL/Queries/OrganizationUnitQuery.cs"
updated: 2026-06-12
---

# OrganizationUnitQuery

GraphQL query resolver that retrieves a filtered list of organisation units from the repository, requiring the Pages_Administration_OrganizationUnits permission.

## Public interface

- `override Task<List<OrganizationUnitDto>> Resolve(IResolveFieldContext context)`

## Dependencies

- [[thinkninsurtech-query-base]] abstract base providing unit-of-work pipeline and AutoMapper projection helpers
- [[context-extensions]] fluent argument handling for filter composition
- [[graphql-organization-unit-dto]] DTO shape returned to the API caller
- [[organization-unit-type]] GraphQL type definition wired into the field type

## Used by

- [[query-container]]

## External dependencies

- Abp.Authorization
- Abp.Domain.Repositories
- GraphQL
- Microsoft.EntityFrameworkCore

## Notes

Supports filtering by id, tenantId and code via fluent ContainsArgument chaining. Uses AsNoTracking for read-only efficiency.
