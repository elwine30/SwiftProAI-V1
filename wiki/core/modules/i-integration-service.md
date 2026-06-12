---
title: "IIntegrationService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Integration/IIntegrationService.cs"
updated: 2026-06-12
---

# IIntegrationService

Contract for making completions requests to ChatGPT (OpenAI) on behalf of the application.

## Public interface

Task<string> ChatGPTCompletions(ChatGPTInputDto input)

## External dependencies

- Abp.Core

## Notes

Thin facade over the OpenAI API. Returns raw string response — structured parsing is handled by callers.
