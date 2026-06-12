---
title: "OpenAIIntegrationLogsAppService"
type: service
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application/Integration/OpenAIIntegrationLogsAppService.cs"
updated: 2026-06-12
---

# OpenAIIntegrationLogsAppService

Provides a paginated, filterable view of OpenAI API call logs enriched with case and organisation unit details for token cost monitoring and audit.

## Public interface

Task<PagedResultDto<GetOpenAIIntegrationLogForViewDto>> GetAll(GetAllOpenAIIntegrationLogsInput input)

## Dependencies

- [[thinkn-insur-tech-app-service-base]] base class providing user, tenant and identity helpers

## External dependencies

- Abp.Zero

## Notes

Joins OpenAIIntegrationLog to MainRegistration using CaseNo cast to int. Exposes PromptToken, CompletionToken and TotalCost fields for per-case cost tracking. Requires Pages_Administration_OpenAIIntegrationLogs permission.
