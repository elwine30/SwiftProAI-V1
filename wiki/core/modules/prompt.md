---
title: "Prompt"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/OCR/Prompt.cs"
updated: 2026-06-12
---

# Prompt

Entity storing named OpenAI prompt templates used by the OCR and AI report generation features.

## Public interface

- `string PromptName` (required)
- `string PromptRequest` — the prompt body sent to the OpenAI API

## External dependencies

- Abp.Zero (FullAuditedEntity, IMayHaveTenant, IMayHaveOrganizationUnit)

## Notes

- Prompts are configurable per tenant/OU, allowing different organisations to customise AI behaviour without code changes.
- `PromptRequest` is unbounded in length — map as `nvarchar(max)` in SQL Server.
