---
title: "MainSchema"
type: class
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.GraphQL/Schemas/MainSchema.cs"
updated: 2026-06-12
---

# MainSchema

Defines the single GraphQL schema for the application, wiring the QueryContainer as the root query type and applying camelCase name conversion.

## Public interface

- `MainSchema(IServiceProvider provider)`

## Dependencies

- [[query-container]] the root query type aggregating all field resolvers

## External dependencies

- Abp.Dependency
- GraphQL

## Notes

No mutation or subscription types are registered; the schema is currently query-only. CamelCaseNameConverter is applied globally.
