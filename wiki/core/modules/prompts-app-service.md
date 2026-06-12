---
title: "PromptsAppService"
type: service
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application/OCR/PromptsAppService.cs"
updated: 2026-06-12
---

# PromptsAppService

Manages OCR prompt templates that are sent to OpenAI for document data extraction, supporting retrieval by name and update of existing prompts.

## Public interface

Task<PagedResultDto<GetPromptForViewDto>> GetAll(GetAllPromptsInput input)
PromptDto GetPromptByPromptName(string promptName)
Task<GetPromptForViewDto> GetPromptForView(int id)
Task<GetPromptForEditOutput> GetPromptForEdit(EntityDto input)
Task CreateOrEdit(CreateOrEditPromptDto input)

## Dependencies

- [[thinkn-insur-tech-app-service-base]] base class providing user, tenant and identity helpers

## External dependencies

- Abp.Zero

## Notes

Create path is commented out — new prompts cannot be created via the API. Queries disable MayHaveTenant filter so host-level prompts are shared across tenants. Requires Pages_Administration_Prompts permission.
