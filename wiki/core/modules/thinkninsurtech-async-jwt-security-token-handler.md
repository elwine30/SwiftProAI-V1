---
title: "ThinknInsurTechAsyncJwtSecurityTokenHandler"
type: class
language: csharp
layer: infrastructure
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/Authentication/JwtBearer/ThinknInsurTechAsyncJwtSecurityTokenHandler.cs"
updated: 2026-06-12
---

# ThinknInsurTechAsyncJwtSecurityTokenHandler

Async token validator that validates JWT access and refresh tokens, checks token type, validates security stamps, and enforces per-token validity keys stored in cache and database.

## Public interface

bool CanValidateToken { get; }
int MaximumTokenSizeInBytes { get; set; }
bool CanReadToken(string securityToken)
Task<(ClaimsPrincipal, SecurityToken)> ValidateToken(string securityToken, TokenValidationParameters validationParameters)
Task<(ClaimsPrincipal, SecurityToken)> ValidateRefreshToken(string securityToken, TokenValidationParameters validationParameters)

## Dependencies

- [[token-auth-configuration]] provides TokenValidationParameters (issuer, audience, signing key)
- [[jwt-security-stamp-handler]] called to validate security stamp on every token validation

## Used by

- [[async-jwt-bearer-options]] pre-seeded into AsyncSecurityTokenValidators list on construction

## External dependencies

- Abp.Runtime.Caching

## Notes

Resolves dependencies directly from IocManager.Instance within validation logic. Also validates user delegation claims inline via PermissionChecker and IUserDelegationManager. Token validity keys are cached to reduce DB lookups on every request.
