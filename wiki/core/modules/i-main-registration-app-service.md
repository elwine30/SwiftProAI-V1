---
title: "IMainRegistrationAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Registration/IMainRegistrationAppService.cs"
updated: 2026-06-12
---

# IMainRegistrationAppService

Contract for the core insurance case registration workflow — creating, listing, reassigning and fetching summary data for claim registrations.

## Public interface

Task<PagedResultDto<MainRegistrationDashboardDto>> GetMainRegistrationDetails(GetMainRegistrationDetailsInput input)
Task<Dictionary<int, int>> GetMainRegistrationDashboardSummary()
Task<int> CreateMainRegistration(CreateMainRegistrationInput registration)
Task UpdateStatus(int registerId)
MainRegistrationDto GetMainRegistrationDetailsByRegisterId(int registerId)
Task UpdateCaseCompany(ReassignCaseCompanyDto data)
Task UpdateCaseAdjuster(ReassignCaseAdjusterDto data)
Task<List<RegistrationCaseTypeLookupTableDto>> GetAllCaseTypeForTableDropdown()
Task<RegistrationCreationTimeMinMax> GetMainRegistrationMinMaxCreationTime()
Task<string> GetMainRegistrationFileRefNo(int registerId)

## Dependencies

- [[main-registration-dto]] DTO returned by GetMainRegistrationDetailsByRegisterId
- [[create-main-registration-input]] input model for CreateMainRegistration

## Used by

- [[main-registration-app-service]]

## External dependencies

- Abp.Core

## Notes

GetMainRegistrationDetailsByRegisterId is synchronous (no Task) — inconsistent with the rest of the interface.
