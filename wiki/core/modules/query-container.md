---
title: "QueryContainer"
type: class
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.GraphQL/Queries/Container/QueryContainer.cs"
updated: 2026-06-12
---

# QueryContainer

Root GraphQL ObjectGraphType that aggregates all individual query field types (roles, users, organisation units) into the single top-level query object used by the schema.

## Public interface

- `QueryContainer(RoleQuery roleQuery, UserQuery userQuery, OrganizationUnitQuery organizationUnitQuery)`

## Dependencies

- [[role-query]] GraphQL resolver for role queries
- [[user-query]] GraphQL resolver for user queries
- [[organization-unit-query]] GraphQL resolver for organisation unit queries

## Used by

- [[main-schema]]

## External dependencies

- Abp.Dependency
- GraphQL
