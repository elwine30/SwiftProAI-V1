---
title: "MainRegistrationAppService"
type: service
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application/Registration/MainRegistrationAppService.cs"
updated: 2026-06-12
---

# MainRegistrationAppService

Orchestrates the full insurance claim registration lifecycle: case creation with auto-generated reference numbers, dashboard querying with role-based data visibility, status progression, and adjuster or company reassignment.

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

- [[thinkn-insur-tech-app-service-base]] base class providing user, tenant and identity helpers

## Used by

- [[i-main-registration-app-service]]

## External dependencies

- Abp.Zero
- System.Linq.Dynamic.Core

## Notes

Contains complex role-based filtering logic in GetMainRegistrationDetails: Superadmin/Admin-no-OU see all; Adjuster-company Admin sees own OU; Third-party Admin sees cases assigned via ViewThirdPartyCases; Adjusters see their own cases. Case reference numbers are auto-generated using prefix and length from DocumentSettings. Disables HaveOrganizationUnit ABP filter for cross-OU queries. Also creates a ViewThirdPartyCases record when insurance company has an AssignOUId.
