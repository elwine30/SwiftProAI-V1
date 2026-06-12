---
title: "IPromptsAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/OCR/IPromptsAppService.cs"
updated: 2026-06-12
---

# IPromptsAppService

CRUD contract for managing named OCR prompt templates used to instruct the AI during document processing.

## Public interface

Task<PagedResultDto<GetPromptForViewDto>> GetAll(GetAllPromptsInput input)
Task<GetPromptForViewDto> GetPromptForView(int id)
Task<GetPromptForEditOutput> GetPromptForEdit(EntityDto input)
Task CreateOrEdit(CreateOrEditPromptDto input)

## Used by

- [[i-ocr-prompt-service]]

## External dependencies

- Abp.Core
