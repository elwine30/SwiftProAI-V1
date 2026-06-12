---
title: "OrganizationUnitDto"
type: class
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.GraphQL/Dto/OrganizationUnitDto.cs"
updated: 2026-06-12
---

# OrganizationUnitDto

Data transfer object representing an organisation unit as returned by the GraphQL API, mapped automatically from the ABP OrganizationUnit domain entity.

## Public interface

- `long Id { get; set; }`
- `string Code { get; set; }`
- `string DisplayName { get; set; }`
- `int? TenantId { get; set; }`

## Used by

- [[organization-unit-query]]
- [[organization-unit-type]]

## External dependencies

- Abp.AutoMapper
- Abp.Organizations
