---
title: "ServiceCollectionExtensions"
type: class
language: csharp
layer: infrastructure
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.GraphQL/Configure/ServiceCollectionExtensions.cs"
updated: 2026-06-12
---

# ServiceCollectionExtensions

Registers and configures GraphQL.NET services into the ASP.NET Core DI container, including JSON serialisation, error detail exposure and synchronous I/O workaround for Kestrel and IIS.

## Public interface

- `static void AddAndConfigureGraphQL(this IServiceCollection services)`

## External dependencies

- GraphQL
- Microsoft.AspNetCore.Server.Kestrel.Core

## Notes

AllowSynchronousIO is explicitly enabled on both Kestrel and IIS — required due to a known graphql-dotnet limitation (issue #1326). DebugHelper.IsDebug controls whether exception details are exposed in error responses.
