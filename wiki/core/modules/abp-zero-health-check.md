---
title: "AbpZeroHealthCheck"
type: class
language: csharp
layer: infrastructure
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/HealthCheck/AbpZeroHealthCheck.cs"
updated: 2026-06-12
---

# AbpZeroHealthCheck

Extension method that registers three health checks (database connection, database with user validation, and cache) on the ASP.NET Core health checks builder.

## Public interface

static IHealthChecksBuilder AddAbpZeroHealthCheck(this IServiceCollection services)

## External dependencies

- Microsoft.Extensions.Diagnostics.HealthChecks

## Notes

Delegates to ThinknInsurTechDbContextHealthCheck, ThinknInsurTechDbContextUsersHealthCheck and CacheHealthCheck which are defined in a different project layer.
