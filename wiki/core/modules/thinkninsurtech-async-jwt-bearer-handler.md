---
title: "ThinknInsurTechAsyncJwtBearerHandler"
type: class
language: csharp
layer: infrastructure
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/Authentication/JwtBearer/ThinknInsurTechAsyncJwtBearerHandler.cs"
updated: 2026-06-12
---

# ThinknInsurTechAsyncJwtBearerHandler

Custom JWT bearer authentication handler that extends the standard ASP.NET Core handler to support async token validators before falling back to synchronous validators.

## Public interface

protected override Task<AuthenticateResult> HandleAuthenticateAsync()
protected override Task HandleChallengeAsync(AuthenticationProperties properties)
protected override Task HandleForbiddenAsync(AuthenticationProperties properties)

## Dependencies

- [[async-jwt-bearer-options]] provides the list of AsyncSecurityTokenValidators iterated during authentication

## Used by

- [[thinkninsurtech-jwt-bearer-extensions]] registered as the handler implementation when AddAbpAsyncJwtBearer is called

## External dependencies

- Microsoft.AspNetCore.Authentication.JwtBearer
- Microsoft.IdentityModel.Protocols.OpenIdConnect

## Notes

Copied and adapted from ASP.NET Core source. Iterates AsyncSecurityTokenValidators first, then falls back to standard SecurityTokenValidators. Handles key rollover by requesting ConfigurationManager refresh on SecurityTokenSignatureKeyNotFoundException.
