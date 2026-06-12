---
title: "IOpenAIIntegrationLogsAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Integration/IOpenAIIntegrationLogsAppService.cs"
updated: 2026-06-12
---

# IOpenAIIntegrationLogsAppService

Read-only service contract for retrieving paginated logs of all OpenAI API calls made by the system.

## Public interface

Task<PagedResultDto<GetOpenAIIntegrationLogForViewDto>> GetAll(GetAllOpenAIIntegrationLogsInput input)

## External dependencies

- Abp.Core
