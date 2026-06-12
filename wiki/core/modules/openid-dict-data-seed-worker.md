---
title: "OpenIdDictDataSeedWorker"
type: service
language: csharp
layer: infrastructure
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/OpenIddict/OpenIdDictDataSeedWorker.cs"
updated: 2026-06-12
---

# OpenIdDictDataSeedWorker

IHostedService that seeds OpenIddict applications and scopes from the OpenIddict:Applications appsettings section at application startup, skipping if OpenIddict is disabled.

## Public interface

Task StartAsync(CancellationToken cancellationToken)
Task StopAsync(CancellationToken cancellationToken)

## Dependencies

- [[app-configuration-accessor]] reads OpenIddict:Applications configuration section to determine which clients to seed

## Used by

- [[openiddict-registrar]] registers this worker as an IHostedService during OpenIddict setup

## External dependencies

- OpenIddict.Abstractions
- Abp.Domain.Uow
