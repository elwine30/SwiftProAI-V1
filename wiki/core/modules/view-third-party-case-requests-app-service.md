---
title: "ViewThirdPartyCaseRequestsAppService"
type: service
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application/Approval/ViewThirdPartyCaseRequestsAppService.cs"
updated: 2026-06-12
---

# ViewThirdPartyCaseRequestsAppService

Handles the onboarding approval workflow by which an adjuster company grants a third-party organisation (insurer, law firm or workshop) visibility of assigned cases.

## Public interface

Task<PagedResultDto<GetViewThirdPartyCaseRequestForViewDto>> GetAllNotOnboarded(GetAllViewThirdPartyCaseRequestsInput input)
Task<PagedResultDto<GetViewThirdPartyCaseRequestForViewDto>> GetAllOnboarded(GetAllViewThirdPartyCaseRequestsInput input)
Task<GetViewThirdPartyCaseRequestForViewDto> GetViewThirdPartyCaseRequestForView(int id)
Task<GetViewThirdPartyCaseRequestForEditOutput> GetViewThirdPartyCaseRequestForEdit(EntityDto input)
Task CreateOrEdit(CreateOrEditViewThirdPartyCaseRequestDto input)
Task Delete(EntityDto input)

## Dependencies

- [[thinkn-insur-tech-app-service-base]] base class providing user, tenant and identity helpers

## External dependencies

- Abp.Zero

## Notes

Update (approval) path resolves company type from DocumentSettings and updates the corresponding master entity (InsuranceCompany, LawFirm or Workshop) with AssignOUId, then bulk-inserts ViewThirdPartyCases records for all existing assigned cases. Class-level AbpAuthorize attribute is commented out.
