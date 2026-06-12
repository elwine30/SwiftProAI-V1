---
title: "ICasePoliceReportsAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Registration/ICasePoliceReportsAppService.cs"
updated: 2026-06-12
---

# ICasePoliceReportsAppService

Contract for recording police reports attached to a case and managing the associated report file uploads.

## Public interface

Task<PagedResultDto<GetCasePoliceReportForViewDto>> GetAll(GetAllCasePoliceReportsInput input)
Task<PagedResultDto<GetCasePoliceReportForViewDto>> GetAllForView(GetAllCasePoliceReportsInput input)
Task<GetCasePoliceReportForViewDto> GetCasePoliceReportForView(int id)
Task<GetCasePoliceReportForEditOutput> GetCasePoliceReportForEdit(GetCasePoliceReportForEditInput input)
Task CreateOrEdit(CreateOrEditCasePoliceReportDto input)
Task Delete(EntityDto input)
Task RemoveReportFileUploadFile(EntityDto input)

## External dependencies

- Abp.Core
