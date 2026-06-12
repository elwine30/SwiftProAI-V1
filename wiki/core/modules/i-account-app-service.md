---
title: "IAccountAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Authorization/Accounts/IAccountAppService.cs"
updated: 2026-06-12
---

# IAccountAppService

Contract for account lifecycle operations including tenant resolution, registration, password reset, email activation and user impersonation.

## Public interface

Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input)
Task<int?> ResolveTenantId(ResolveTenantIdInput input)
Task<RegisterOutput> Register(RegisterInput input)
Task SendPasswordResetCode(SendPasswordResetCodeInput input)
Task<ResetPasswordOutput> ResetPassword(ResetPasswordInput input)
Task SendEmailActivationLink(SendEmailActivationLinkInput input)
Task<ImpersonateOutput> ImpersonateUser(ImpersonateUserInput input)
Task<ImpersonateOutput> ImpersonateTenant(ImpersonateTenantInput input)
Task<ImpersonateOutput> DelegatedImpersonate(DelegatedImpersonateInput input)
Task<SwitchToLinkedAccountOutput> SwitchToLinkedAccount(SwitchToLinkedAccountInput input)

## External dependencies

- Abp.Core
