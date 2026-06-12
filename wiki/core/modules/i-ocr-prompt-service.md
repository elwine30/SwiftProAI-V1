---
title: "IOCRPromptService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Integration/IOCRPromptService.cs"
updated: 2026-06-12
---

# IOCRPromptService

Contract for uploading documents to the OCR pipeline and generating AI-based summaries from extracted content.

## Public interface

Task<string> UploadDocument(UploadDocTypeEnum docType, string imageBase64String, string filename, string caseNo)
Task<string> UploadBulkDocument(UploadDocTypeEnum docType, string[] base64Files, string filename, string caseNo)
Task<string> GenerateSummary(string inputData, string promptName, string caseNo)

## Dependencies

- [[i-prompts-app-service]] used to resolve named prompt templates for GenerateSummary

## Notes

Does not inherit IApplicationService — intended as an internal infrastructure service rather than an exposed ABP app service. Uses UploadDocTypeEnum from Registration namespace.
