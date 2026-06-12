---
title: "IViewThirdPartyCaseRequestsAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Approval/IViewThirdPartyCaseRequestsAppService.cs"
updated: 2026-06-12
---

# IViewThirdPartyCaseRequestsAppService

Contract for the approval queue that lists and processes third-party case access requests not yet onboarded.

## Public interface

Task<PagedResultDto<GetViewThirdPartyCaseRequestForViewDto>> GetAllNotOnboarded(GetAllViewThirdPartyCaseRequestsInput input)
Task<GetViewThirdPartyCaseRequestForViewDto> GetViewThirdPartyCaseRequestForView(int id)
Task<GetViewThirdPartyCaseRequestForEditOutput> GetViewThirdPartyCaseRequestForEdit(EntityDto input)
Task CreateOrEdit(CreateOrEditViewThirdPartyCaseRequestDto input)
Task Delete(EntityDto input)

## Used by

- [[view-third-party-case-requests-app-service]]

## External dependencies

- Abp.Core

## Notes

GetAllNotOnboarded specifically filters to pending requests — approval triggers onboarding via IOUOnboardingAppService.
