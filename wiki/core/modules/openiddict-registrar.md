---
title: "OpenIddictRegistrar"
type: class
language: csharp
layer: infrastructure
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/OpenIddict/OpenIddictRegistrar.cs"
updated: 2026-06-12
---

# OpenIddictRegistrar

Static helper that configures the OpenIddict server (password flow, auth code flow, endpoints) and validation pipeline, and registers the data seed hosted service.

## Public interface

static void Register(IServiceCollection services, IConfigurationRoot configuration, Action<OpenIddictCoreOptions> setupOptions)
static void Register(IServiceCollection services, IConfigurationRoot configuration)

## Dependencies

- [[openid-dict-data-seed-worker]] registered as IHostedService to seed applications and scopes at startup
- [[abp-open-iddict-claims-principal-manager]] registered as the claims principal pipeline manager

## Used by

- [[startup]] calls Register during ConfigureServices to wire OpenIddict into the DI container

## External dependencies

- OpenIddict.Core
- OpenIddict.Server.AspNetCore
- OpenIddict.Validation.AspNetCore

## Notes

Calls AddDevelopmentEncryptionCertificate and AddDevelopmentSigningCertificate — these must be replaced with real certificates in production. Access token encryption is explicitly disabled via DisableAccessTokenEncryption.
