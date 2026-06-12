---
title: "UserPagedResultGraphType"
type: class
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.GraphQL/Types/UserPagedResultGraphType.cs"
updated: 2026-06-12
---

# UserPagedResultGraphType

GraphQL ObjectGraphType wrapping a paginated user result set, exposing a total count and a list of UserType items.

## Public interface

- `UserPagedResultGraphType()`

## Dependencies

- [[user-type]] GraphQL type for individual user items in the list field
- [[graphql-user-dto]] DTO type used as the generic parameter for the paged result

## Used by

- [[user-query]]

## External dependencies

- Abp.Application.Services.Dto
- GraphQL
