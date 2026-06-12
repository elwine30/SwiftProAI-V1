---
title: "ITenantAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/MultiTenancy/ITenantAppService.cs"
updated: 2026-06-12
---

# ITenantAppService

Host-level contract for managing tenants including creation, feature assignment and unlocking tenant administrators.

## Public interface

Task<PagedResultDto<TenantListDto>> GetTenants(GetTenantsInput input)
Task CreateTenant(CreateTenantInput input)
Task<TenantEditDto> GetTenantForEdit(EntityDto input)
Task UpdateTenant(TenantEditDto input)
Task DeleteTenant(EntityDto input)
Task<GetTenantFeaturesEditOutput> GetTenantFeaturesForEdit(EntityDto input)
Task UpdateTenantFeatures(UpdateTenantFeaturesInput input)
Task ResetTenantSpecificFeatures(EntityDto input)
Task UnlockTenantAdmin(EntityDto input)

## External dependencies

- Abp.Core
