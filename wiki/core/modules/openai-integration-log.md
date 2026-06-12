---
title: "OpenAIIntegrationLog"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Integration/OpenAIIntegrationLog.cs"
updated: 2026-06-12
---

# OpenAIIntegrationLog

Entity persisting each request/response pair made to the OpenAI API, including token counts and cost, for audit and cost-tracking purposes.

## Public interface

- `string ActionUrl` (required)
- `string Request` (required) — the prompt sent
- `string Response` — the completion returned
- `int PromptToken` / `int CompletionToken`
- `decimal TotalCost`
- `string CaseNo` — the case this OCR call relates to

## External dependencies

- Abp.Zero (CreationAuditedEntity)

## Notes

- Not tenant-scoped at the entity level — tenant filtering relies on ABP session context in `OpenAIIntegrationLogManager`.
- `TotalCost` is a pre-calculated decimal; the cost formula is applied in the application service layer.
