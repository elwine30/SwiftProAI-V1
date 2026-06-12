---
title: "CustomDtoMapper"
type: class
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.GraphQL/Startup/CustomDtoMapper.cs"
updated: 2026-06-12
---

# CustomDtoMapper

Provides the AutoMapper profile configuration for mapping the User domain entity to UserDto, explicitly ignoring the Roles and OrganizationUnits collections that are populated separately by UserQuery.

## Public interface

- `static void CreateMappings(IMapperConfigurationExpression configuration)`

## Dependencies

- [[graphql-user-dto]] target DTO type for the User entity mapping

## Used by

- [[thinkninsurtech-graph-ql-module]]

## External dependencies

- AutoMapper

## Notes

Roles and OrganizationUnits are ignored in this mapping because UserQuery populates them via separate repository lookups after the main query.
