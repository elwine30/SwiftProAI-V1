---
title: "ICaseDebitNotesAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Registration/ICaseDebitNotesAppService.cs"
updated: 2026-06-12
---

# ICaseDebitNotesAppService

Contract for managing debit note documents that add charges to outstanding invoice balances on a case.

## Public interface

Task<GetCaseDebitNoteForPreviewDto> GetCaseDebitNoteForPreview(int id)
Task<GetCaseDebitNoteForViewDto> GetCaseDebitNoteForView(int id)
Task<GetCaseDebitNoteForEditOutput> GetCaseDebitNoteForEdit(EntityDto input)
Task CreateOrEdit(CreateOrEditCaseDebitNoteDto input)
Task Delete(EntityDto input)
Task<List<CaseDebitNoteMainRegistrationLookupTableDto>> GetAllMainRegistrationForTableDropdown()

## External dependencies

- Abp.Core

## Notes

File has five duplicate `using System.Collections.Generic;` statements — likely a code generation artefact.
