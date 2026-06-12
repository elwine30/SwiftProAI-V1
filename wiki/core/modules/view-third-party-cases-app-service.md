---
title: "ViewThirdPartyCasesAppService"
type: service
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application/Organizations/ViewThirdPartyCasesAppService.cs"
updated: 2026-06-12
---

# ViewThirdPartyCasesAppService

Manages the ViewThirdPartyCases join table and provides utilities to synchronise AssignOUId on master data and back-fill ViewThirdPartyCases records after a business registration approval.

## Public interface

Task<PagedResultDto<GetViewThirdPartyCasesForViewDto>> GetAll(GetAllViewThirdPartyCasesInput input)
Task<GetViewThirdPartyCasesForEditOutput> GetViewThirdPartyCasesForEdit(EntityDto input)
Task CreateOrEdit(CreateOrEditViewThirdPartyCaseDto input)
Task Delete(EntityDto input)
void SyncAssignOUIdMasterData(string businessRegistrationNo, long ouId, string caseType)
async void SyncThirdPartyCases(string businessRegistrationNo)

## Dependencies

- [[thinkn-insur-tech-app-service-base]] base class providing user, tenant and identity helpers

## External dependencies

- Abp.Zero
- Z.EntityFramework.Plus

## Notes

SyncThirdPartyCases uses async void (fire-and-forget) which suppresses exceptions — marked as 'pending integration and test' in comments. Uses Z.EntityFramework.Plus bulk Update for SyncAssignOUIdMasterData. GetAll returns minimal data only.
