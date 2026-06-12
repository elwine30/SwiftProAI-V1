---
title: "ICaseSearchFeesAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Registration/ICaseSearchFeesAppService.cs"
updated: 2026-06-12
---

# ICaseSearchFeesAppService

Contract for recording and retrieving search fee charges billed during the investigation of a case.

## Public interface

Task<PagedResultDto<GetCaseSearchFeeForViewDto>> GetAll(GetAllCaseSearchFeesInput input)
Task<GetCaseSearchFeeForEditOutput> GetCaseSearchFeeForEdit(EntityDto input)
Task CreateOrEdit(CreateOrEditCaseSearchFeeDto input)
Task Delete(EntityDto input)
Task<List<CaseSearchFeeDto>> GetCaseSearchFeeAmountByRegisterId(EntityDto input)

## External dependencies

- Abp.Core
