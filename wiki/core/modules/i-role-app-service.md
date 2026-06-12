---
title: "IRoleAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Authorization/Roles/IRoleAppService.cs"
updated: 2026-06-12
---

# IRoleAppService

Contract for role management — listing, creating, updating and deleting ABP roles with associated permissions.

## Public interface

Task<ListResultDto<RoleListDto>> GetRoles(GetRolesInput input)
Task<GetRoleForEditOutput> GetRoleForEdit(NullableIdDto input)
Task CreateOrUpdateRole(CreateOrUpdateRoleInput input)
Task DeleteRole(EntityDto input)

## External dependencies

- Abp.Core
