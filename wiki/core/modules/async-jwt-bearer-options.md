---
title: "AsyncJwtBearerOptions"
type: class
language: csharp
layer: infrastructure
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/Authentication/JwtBearer/AsyncJwtBearerOptions.cs"
updated: 2026-06-12
---

# AsyncJwtBearerOptions

Extends JwtBearerOptions to hold a list of async security token validators, pre-seeded with the custom ThinknInsurTechAsyncJwtSecurityTokenHandler.

## Public interface

List<IAsyncSecurityTokenValidator> AsyncSecurityTokenValidators

## Dependencies

- [[thinkninsurtech-async-jwt-security-token-handler]] instantiated and added to AsyncSecurityTokenValidators in the constructor

## Used by

- [[token-auth-controller]] accesses async validator list for token validation during authentication
- [[thinkninsurtech-async-jwt-bearer-handler]] reads AsyncSecurityTokenValidators during HandleAuthenticateAsync
- [[thinkninsurtech-jwt-bearer-extensions]] passed as the options type when registering the handler with DI

## External dependencies

- Microsoft.AspNetCore.Authentication.JwtBearer
