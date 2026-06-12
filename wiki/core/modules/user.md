---
title: "User"
type: class
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/Authorization/Users/User.cs"
updated: 2026-06-12
---

# User

Application user entity extending ABP's `AbpUser<User>` with platform-specific fields for profile photos, sign-in tokens and two-factor authentication.

## Public interface

- `Guid? ProfilePictureId` — BinaryObject reference for profile image
- `bool ShouldChangePasswordOnNextLogin`
- `string SignInToken` / `DateTime? SignInTokenExpireTimeUtc`
- `string GoogleAuthenticatorKey` / `string RecoveryCode`
- `List<UserOrganizationUnit> OrganizationUnits`
- `static User CreateTenantAdminUser(int tenantId, string emailAddress, string name, string surname)`
- `static User CreateSuperAdminUser(int tenantId, string userName, string emailAddress, ...)`
- `void Unlock()` — resets `AccessFailedCount` and `LockoutEndDateUtc`
- `void SetSignInToken()` — generates a 1-minute expiry sign-in token

## External dependencies

- Abp.Zero (AbpUser<User>)
- Abp.Timing

## Notes

- Lockout is enabled by default (`IsLockoutEnabled = true`) and two-factor authentication is on by default (`IsTwoFactorEnabled = true`).
- `PasswordResetCode` is truncated to 10 characters (uppercase) for mobile-friendly entry — overrides the ABP default.
- Referenced widely as FK by `MainRegistration`, `CaseAdjuster`, `CaseInvoice`, `Staff` and others.
