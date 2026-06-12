---
title: "IDocumentSettingsAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Common/IDocumentSettingsAppService.cs"
updated: 2026-06-12
---

# IDocumentSettingsAppService

Contract for managing document configuration settings that control upload rules and folder structures per document type.

## Public interface

Task<PagedResultDto<GetDocumentSettingForViewDto>> GetAll(GetAllDocumentSettingsInput input)
Task<GetDocumentSettingForViewDto> GetDocumentSettingForView(int id)
Task<GetDocumentSettingForEditOutput> GetDocumentSettingForEdit()
Task CreateOrEdit(CreateOrEditDocumentSettingDto input)
Task Delete(EntityDto input)

## Used by

- [[document-settings-app-service]]

## External dependencies

- Abp.Core

## Notes

GetDocumentSettingForEdit takes no parameters — implies a singleton settings record rather than per-entity settings.
