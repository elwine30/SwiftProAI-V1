---
title: "RoleDto"
type: class
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.GraphQL/Dto/RoleDto.cs"
updated: 2026-06-12
---

# RoleDto

Data transfer object representing an application role as returned by the GraphQL API, mapped automatically from the Role domain entity.

## Public interface

- `int Id { get; set; }`
- `string Name { get; set; }`
- `string DisplayName { get; set; }`
- `bool IsStatic { get; set; }`
- `bool IsDefault { get; set; }`
- `DateTime CreationTime { get; set; }`
- `int? TenantId { get; set; }`

## Dependencies

- [[authorization-roles]] provides the role permission constants mapped to the IsStatic flag

## Used by

- [[role-query]]
- [[role-type]]

## External dependencies

- Abp.AutoMapper
