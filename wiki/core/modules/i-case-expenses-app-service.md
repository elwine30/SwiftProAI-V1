---
title: "ICaseExpensesAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Registration/ICaseExpensesAppService.cs"
updated: 2026-06-12
---

# ICaseExpensesAppService

Contract for recording, retrieving and updating adjuster expense claims against a registered insurance case.

## Public interface

Task<PagedResultDto<GetCaseExpenseForViewDto>> GetAll(GetAllCaseExpensesInput input)
Task<GetCaseExpenseForViewDto> GetCaseExpenseForView(int id)
Task Delete(EntityDto input)
Task<GetCaseExpenseAdjusterViewDto> CreateExpenses(CreateExpenseInput input)
Task UpdateCaseExpenses(UpdateCaseExpensesDTO input)
Task<List<GetCaseExpenseAdjusterViewDto>> GetCaseExpenseAdjusterViewDto(int id)
Task<List<CaseExpenseLookupTableDto>> GetExpensesTypeList(string lookupGroup)

## Dependencies

- [[update-case-expenses-dto]] input DTO for UpdateCaseExpenses

## External dependencies

- Abp.Core

## Notes

Contains a dedicated #region for adjuster-facing operations, indicating dual-audience usage.
