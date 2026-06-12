---
title: "ThinknInsurTechQueryBase"
type: class
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.GraphQL/Core/Base/ThinknInsurTechQueryBase.cs"
updated: 2026-06-12
---

# ThinknInsurTechQueryBase

Abstract base class for all GraphQL query resolvers, providing field-type construction, argument management, AutoMapper projection helpers and a [UnitOfWork]-wrapped resolve pipeline.

## Public interface

- `Dictionary<string, Type> Arguments { get; set; }`
- `string FieldName { get; set; }`
- `ResolveFieldContext<object> Context { get; set; }`
- `List<QueryArgument> GetQueryArguments()`
- `abstract Task<TResult> Resolve(IResolveFieldContext context)`
- `FieldType GetFieldType()`
- `IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source)`
- `Task<List<TDestination>> ProjectToListAsync<TDestination>(IQueryable source)`

## Used by

- [[organization-unit-query]]
- [[role-query]]
- [[user-query]]

## External dependencies

- Abp.Authorization
- Abp.Dependency
- Abp.Domain.Uow
- AutoMapper
- GraphQL
- Microsoft.EntityFrameworkCore

## Notes

InternalResolve is decorated with [UnitOfWork] so every concrete resolver participates in ABP's unit-of-work pipeline. IPermissionChecker defaults to NullPermissionChecker, meaning permission checks must be applied by concrete subclasses via [AbpAuthorize].
