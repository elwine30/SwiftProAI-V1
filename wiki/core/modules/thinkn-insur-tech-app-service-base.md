---
title: "ThinknInsurTechAppServiceBase"
type: class
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application/ThinknInsurTechAppServiceBase.cs"
updated: 2026-06-12
---

# ThinknInsurTechAppServiceBase

Abstract base class for all application services, providing helpers to resolve the current user, current tenant, and Identity error checking.

## Public interface

TenantManager TenantManager { get; set; }
UserManager UserManager { get; set; }
Task<User> GetCurrentUserAsync()
User GetCurrentUser()
Task<Tenant> GetCurrentTenantAsync()
Tenant GetCurrentTenant()
void CheckErrors(IdentityResult identityResult)

## Used by

- [[main-registration-app-service]]
- [[case-claims-app-service]]
- [[case-invoices-app-service]]
- [[case-adjusters-app-service]]
- [[case-lawyers-app-service]]
- [[case-workshops-app-service]]
- [[case-insurers-app-service]]
- [[case-third-party-infos-app-service]]
- [[registration-exporter-app-service]]
- [[company-app-service]]
- [[view-third-party-case-requests-app-service]]
- [[view-third-party-cases-app-service]]
- [[ou-onboarding-app-service]]
- [[open-ai-integration-logs-app-service]]
- [[prompts-app-service]]
- [[case-reports-app-service]]
- [[wip-reports-app-service]]
- [[adjuster-reports-app-service]]
- [[case-type-app-service]]
- [[document-settings-app-service]]
- [[common-dropdown-app-service]]
- [[remark-app-service]]
- [[law-firms-app-service]]
- [[branch-app-service]]

## External dependencies

- Abp.Zero

## Notes

Inherits ABP ApplicationService. All domain app services in this project derive from this class.
