---
title: "ThinknInsurTechGraphQLModule"
type: module
language: csharp
layer: infrastructure
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.GraphQL/Startup/ThinknInsurTechGraphQLModule.cs"
updated: 2026-06-12
---

# ThinknInsurTechGraphQLModule

ABP module class for the GraphQL project, declaring a dependency on ThinknInsurTechCoreModule, registering all types by convention and hooking in the custom AutoMapper configurator.

## Public interface

- `override void Initialize()`
- `override void PreInitialize()`

## Dependencies

- [[graphql-custom-dto-mapper]] registers the User-to-UserDto AutoMapper profile during module pre-initialisation

## External dependencies

- Abp.AutoMapper
- Abp.Modules
