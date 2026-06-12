---
title: "AppConfigurationAccessor"
type: service
language: csharp
layer: infrastructure
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/Configuration/AppConfigurationAccessor.cs"
updated: 2026-06-12
---

# AppConfigurationAccessor

Singleton that exposes the resolved IConfigurationRoot to application code via the IAppConfigurationAccessor interface, loaded from the hosting environment at startup.

## Public interface

IConfigurationRoot Configuration { get; }

## Used by

- [[web-core-module]] injects accessor to read JWT and folder settings during module initialisation
- [[openid-dict-data-seed-worker]] reads OpenIddict:Applications section to seed OAuth clients
- [[file-controller]] reads Folder.root for FileOrg path construction

## External dependencies

- Abp.Dependency
- Microsoft.AspNetCore.Hosting
