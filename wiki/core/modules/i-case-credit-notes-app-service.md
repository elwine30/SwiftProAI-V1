---
title: "ICaseCreditNotesAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Registration/ICaseCreditNotesAppService.cs"
updated: 2026-06-12
---

# ICaseCreditNotesAppService

Contract for managing credit note documents that reduce outstanding invoice amounts on a case.

## Public interface

Task<GetCaseCreditNoteForPreviewDto> GetCaseCreditNoteForPreview(int id)
Task<GetCaseCreditNoteForViewDto> GetCaseCreditNoteForView(int id)
Task<GetCaseCreditNoteForEditOutput> GetCaseCreditNoteForEdit(EntityDto input)
Task CreateOrEdit(CreateOrEditCaseCreditNoteDto input)
Task Delete(EntityDto input)
Task<List<CaseCreditNoteMainRegistrationLookupTableDto>> GetAllMainRegistrationForTableDropdown()

## External dependencies

- Abp.Core
