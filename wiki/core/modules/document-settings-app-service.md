---
title: "DocumentSettingsAppService"
type: service
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application/Common/DocumentSettingsAppService.cs"
updated: 2026-06-12
---

# DocumentSettingsAppService

Manages per-organisation document settings including legal name, business registration number, tax number, and reference number prefixes and lengths for invoices, debit notes, credit notes and case files.

## Public interface

Task<PagedResultDto<GetDocumentSettingForViewDto>> GetAll(GetAllDocumentSettingsInput input)
Task<GetDocumentSettingForViewDto> GetDocumentSettingForView(int id)
Task<GetDocumentSettingForEditOutput> GetDocumentSettingForEdit()
Task CreateOrEdit(CreateOrEditDocumentSettingDto input)
Task Delete(EntityDto input)

## Dependencies

- [[thinkn-insur-tech-app-service-base]] base class providing user, tenant and identity helpers

## External dependencies

- Abp.Zero

## Notes

GetDocumentSettingForEdit resolves the current user's OU automatically — if no OU is found it throws a UserFriendlyException. Create validates uniqueness of businessRegistrationNo and caseRefNoPrefix across all tenants by disabling HaveOrganizationUnit filter. Note on OU design: assumes one OU per user.
