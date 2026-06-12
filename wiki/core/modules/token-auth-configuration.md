---
title: "TokenAuthConfiguration"
type: class
language: csharp
layer: infrastructure
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/Authentication/JwtBearer/TokenAuthConfiguration.cs"
updated: 2026-06-12
---

# TokenAuthConfiguration

Configuration value object holding JWT signing key, issuer, audience, signing credentials and token expiration durations populated from appsettings at startup.

## Public interface

SymmetricSecurityKey SecurityKey { get; set; }
string Issuer { get; set; }
string Audience { get; set; }
SigningCredentials SigningCredentials { get; set; }
TimeSpan AccessTokenExpiration { get; set; }
TimeSpan RefreshTokenExpiration { get; set; }

## Used by

- [[web-core-module]] populates this object during PreInitialize via ConfigureTokenAuth
- [[token-auth-controller]] reads signing credentials and expiration values to issue tokens
- [[thinkninsurtech-async-jwt-security-token-handler]] reads SecurityKey and Issuer for token validation

## External dependencies

- Microsoft.IdentityModel.Tokens
