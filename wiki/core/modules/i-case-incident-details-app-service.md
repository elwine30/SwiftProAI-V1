---
title: "ICaseIncidentDetailsAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Registration/ICaseIncidentDetailsAppService.cs"
updated: 2026-06-12
---

# ICaseIncidentDetailsAppService

Contract for capturing and retrieving narrative incident details for a case, including circumstance file uploads.

## Public interface

Task<GetCaseIncidentDetailForEditOutput> GetCaseIncidentDetailForEdit(EntityDto input)
Task CreateOrEdit(CreateOrEditCaseIncidentDetailDto input)
Task<GetCaseIncidentDetailForViewDto> GetCaseIncidentDetailForView(int id)
Task Delete(EntityDto input)
Task<List<CaseIncidentDetailMainRegistrationLookupTableDto>> GetAllMainRegistrationForTableDropdown()
Task RemoveCircumstancesFileUploadFile(EntityDto input)
Task<GetCaseIncidentDetailForEditOutput> GetOneData(int mainId)

## External dependencies

- Abp.Core
