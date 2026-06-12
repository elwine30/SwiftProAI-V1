---
title: "IUserDelegationAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Authorization/Users/Delegation/IUserDelegationAppService.cs"
updated: 2026-06-12
---

# IUserDelegationAppService

Contract for the user delegation feature allowing one user to act on behalf of another for a defined period.

## Public interface

Task<PagedResultDto<UserDelegationDto>> GetDelegatedUsers(GetUserDelegationsInput input)
Task DelegateNewUser(CreateUserDelegationDto input)
Task RemoveDelegation(EntityDto<long> input)
Task<List<UserDelegationDto>> GetActiveUserDelegations()

## External dependencies

- Abp.Core
