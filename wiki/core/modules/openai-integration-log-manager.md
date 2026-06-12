---
title: "OpenAIIntegrationLogManager"
type: service
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Integration/OpenAIIntegrationLogManager.cs"
updated: 2026-06-12
---

# OpenAIIntegrationLogManager

Domain service for persisting OpenAI API interaction logs.

## Public interface

- `Task CreateOpenAIIntegrationLog(OpenAIIntegrationLog openAIIntegrationLog)`

## Dependencies

- [[openai-integration-log]] entity
- [[thinkninsurtech-domain-service-base]] base class

## External dependencies

- Abp.Zero (IRepository, IUnitOfWorkManager, ITenantCache)

## Notes

- `ITenantCache` is injected but not used in the current implementation — likely reserved for future per-tenant cost reporting.
