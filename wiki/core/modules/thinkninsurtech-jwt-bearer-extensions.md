---
title: "ThinknInsurTechJwtBearerExtensions"
type: class
language: csharp
layer: infrastructure
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/Authentication/JwtBearer/ThinknInsurTechJwtBearerExtensions.cs"
updated: 2026-06-12
---

# ThinknInsurTechJwtBearerExtensions

Provides AddAbpAsyncJwtBearer extension methods on AuthenticationBuilder to register the custom async JWT handler and options with the DI pipeline.

## Public interface

static AuthenticationBuilder AddAbpAsyncJwtBearer(this AuthenticationBuilder builder)
static AuthenticationBuilder AddAbpAsyncJwtBearer(this AuthenticationBuilder builder, Action<AsyncJwtBearerOptions> configureOptions)
static AuthenticationBuilder AddAbpAsyncJwtBearer(this AuthenticationBuilder builder, string authenticationScheme, Action<AsyncJwtBearerOptions> configureOptions)
static AuthenticationBuilder AddAbpAsyncJwtBearer(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<AsyncJwtBearerOptions> configureOptions)

## Dependencies

- [[async-jwt-bearer-options]] registered as the options type for the authentication scheme
- [[thinkninsurtech-async-jwt-bearer-handler]] registered as the handler implementation for the scheme

## External dependencies

- Microsoft.AspNetCore.Authentication.JwtBearer

## Notes

Copied and adapted from ASP.NET Core source. Registers JwtBearerPostConfigureOptions as a singleton post-configure step.
