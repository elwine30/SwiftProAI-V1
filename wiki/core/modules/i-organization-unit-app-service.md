---
title: "IOrganizationUnitAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Organizations/IOrganizationUnitAppService.cs"
updated: 2026-06-12
---

# IOrganizationUnitAppService

Contract for managing the hierarchical organisation unit tree including user and role membership within each unit.

## Public interface

Task<ListResultDto<OrganizationUnitDto>> GetOrganizationUnits()
Task<PagedResultDto<OrganizationUnitUserListDto>> GetOrganizationUnitUsers(GetOrganizationUnitUsersInput input)
Task<OrganizationUnitDto> CreateOrganizationUnit(CreateOrganizationUnitInput input)
Task<OrganizationUnitDto> UpdateOrganizationUnit(UpdateOrganizationUnitInput input)
Task<OrganizationUnitDto> MoveOrganizationUnit(MoveOrganizationUnitInput input)
Task DeleteOrganizationUnit(EntityDto<long> input)
Task AddUsersToOrganizationUnit(UsersToOrganizationUnitInput input)
Task AddRolesToOrganizationUnit(RolesToOrganizationUnitInput input)
Task<PagedResultDto<FindOrganizationUnitUsersOutputDto>> FindUsers(FindOrganizationUnitUsersInput input)
Task<List<OrganizationUnitDto>> GetAll()

## External dependencies

- Abp.Core
