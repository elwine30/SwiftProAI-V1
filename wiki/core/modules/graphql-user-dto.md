---
title: "UserDto"
type: class
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.GraphQL/Dto/UserDto.cs"
updated: 2026-06-12
---

# UserDto

Data transfer object representing a user as returned by the GraphQL API, including nested role and organisation unit collections; mapping is defined manually in CustomDtoMapper rather than via attribute.

## Public interface

- `long Id { get; set; }`
- `string Name { get; set; }`
- `string UserName { get; set; }`
- `string EmailAddress { get; set; }`
- `bool IsActive { get; set; }`
- `IEnumerable<RoleDto> Roles { get; set; }`
- `IEnumerable<OrganizationUnitDto> OrganizationUnits { get; set; }`

## Dependencies

- [[graphql-custom-dto-mapper]] defines the AutoMapper profile for mapping User to this DTO

## Used by

- [[user-query]]
- [[graphql-custom-dto-mapper]]
- [[user-paged-result-graph-type]]
- [[user-type]]

## External dependencies

- Abp.AutoMapper
- Abp.Domain.Entities
- Abp.Domain.Entities.Auditing

## Notes

Contains nested inner classes UserDto.RoleDto and UserDto.OrganizationUnitDto used specifically within UserType. The outer Roles and OrganizationUnits collections are ignored during AutoMapper mapping and populated manually in UserQuery.
