---
title: "TokenAuthController"
type: controller
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/Controllers/TokenAuthController.cs"
updated: 2026-06-12
---

# TokenAuthController

REST controller that handles JWT-based authentication including standard login, refresh token, two-factor auth, external OAuth login, impersonation, user delegation and linked-account switching.

## Public interface

Task<AuthenticateResultModel> Authenticate([FromBody] AuthenticateModel model)
Task<RefreshTokenResult> RefreshToken(string refreshToken)
Task LogOut()
Task SendTwoFactorAuthCode([FromBody] SendTwoFactorAuthCodeModel model)
Task<ImpersonatedAuthenticateResultModel> ImpersonatedAuthenticate(string impersonationToken)
Task<ImpersonatedAuthenticateResultModel> DelegatedImpersonatedAuthenticate(long userDelegationId, string impersonationToken)
Task<SwitchedAccountAuthenticateResultModel> LinkedAccountAuthenticate(string switchAccountToken)
List<ExternalLoginProviderInfoModel> GetExternalAuthenticationProviders()
Task<ExternalAuthenticateResultModel> ExternalAuthenticate([FromBody] ExternalAuthenticateModel model)
Task<ActionResult> TestNotification(string message, string severity)

## Dependencies

- [[token-auth-configuration]] provides signing credentials and expiration durations for token generation
- [[async-jwt-bearer-options]] used to access async token validators for validation pipeline
- [[jwt-security-stamp-handler]] validates and updates security stamps on login and logout
- [[two-factor-code-cache-extensions]] reads and clears cached two-factor codes
- [[external-login-info-manager-factory]] resolves correct IExternalLoginInfoManager per OAuth provider
- [[web-consts]] references ReCaptchaIgnoreWhiteList for bypass logic

## Used by

- [[web-core-module]] registers this controller as part of the web core module assembly

## External dependencies

- Abp.AspNetCore
- Abp.AspNetZeroCore.Web

## Notes

RecaptchaValidator is property-injected (defaults to NullRecaptchaValidator). Concurrent login enforcement is controlled by AppSettings.UserManagement.AllowOneConcurrentLoginPerUser. EncryptQueryParameters uses SimpleStringCipher for password-reset link parameters.
