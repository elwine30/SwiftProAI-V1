---
title: "IUserAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Authorization/Users/IUserAppService.cs"
updated: 2026-06-12
---

# IUserAppService

Contract for full user management including CRUD, permission assignment, Excel export and a dedicated onboarding user creation path.

## Public interface

Task<PagedResultDto<UserListDto>> GetUsers(GetUsersInput input)
Task<FileDto> GetUsersToExcel(GetUsersToExcelInput input)
Task<GetUserForEditOutput> GetUserForEdit(NullableIdDto<long> input)
Task<GetUserPermissionsForEditOutput> GetUserPermissionsForEdit(EntityDto<long> input)
Task UpdateUserPermissions(UpdateUserPermissionsInput input)
Task<long> CreateOrUpdateUser(CreateOrUpdateUserInput input)
Task<long> CreateOnboardingUser(CreateOrUpdateUserInput input)
Task DeleteUser(EntityDto<long> input)
Task UnlockUser(EntityDto<long> input)

## External dependencies

- Abp.Core

## Notes

CreateOnboardingUser is a separate path from CreateOrUpdateUser, likely used during the OU onboarding flow to skip certain validation steps.
