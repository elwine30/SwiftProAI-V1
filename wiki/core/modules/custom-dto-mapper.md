---
title: "CustomDtoMapper"
type: class
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application/CustomDtoMapper.cs"
updated: 2026-06-12
---

# CustomDtoMapper

Central AutoMapper profile that declares all entity-to-DTO and DTO-to-entity mappings for the entire application layer.

## Public interface

static void CreateMappings(IMapperConfigurationExpression configuration)

## External dependencies

- AutoMapper

## Notes

All mappings must be declared here per project conventions. No inline mapping is permitted in AppService classes.
