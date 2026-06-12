---
title: "IProfileAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Authorization/Users/Profile/IProfileAppService.cs"
updated: 2026-06-12
---

# IProfileAppService

Contract for current-user profile management including password changes, profile picture, SMS verification and Google Authenticator MFA setup.

## Public interface

Task<CurrentUserProfileEditDto> GetCurrentUserProfileForEdit()
Task UpdateCurrentUserProfile(CurrentUserProfileEditDto input)
Task ChangePassword(ChangePasswordInput input)
Task UpdateProfilePicture(UpdateProfilePictureInput input)
Task<GetProfilePictureOutput> GetProfilePictureByUserName(string username)
Task SendVerificationSms(SendVerificationSmsInputDto input)
Task VerifySmsCode(VerifySmsCodeInputDto input)
Task<GenerateGoogleAuthenticatorKeyOutput> GenerateGoogleAuthenticatorKey()
Task<bool> VerifyAuthenticatorCode(VerifyAuthenticatorCodeInput input)
Task PrepareCollectedData()

## External dependencies

- Abp.Core
