---
title: "AuthConfigurer"
type: class
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Host/Startup/AuthConfigurer.cs"
updated: 2026-06-12
---

# AuthConfigurer

Configures JWT Bearer authentication and resolves encrypted query-string tokens for SignalR and profile-picture endpoints that cannot send an Authorization header.

## Public interface

- `static void Configure(IServiceCollection services, IConfiguration configuration)`

## Used by

- [[startup]]

## External dependencies

- Abp.AspNetZeroCore.Web
- Microsoft.AspNetCore.Authentication.JwtBearer
- Microsoft.IdentityModel.Tokens

## Notes

ClockSkew is set to zero for strict token expiry. The enc_auth_token query parameter is decrypted using SimpleStringCipher with DefaultPassPhrase — this applies to /signalr* and /Chat/GetUploadedObject and /Profile/GetProfilePictureByUser.
