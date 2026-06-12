---
title: "TenantBasedWsFederationExternalLoginInfoProvider"
type: class
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Host/Startup/ExternalLoginInfoProviders/TenantBasedWsFederationExternalLoginInfoProvider.cs"
updated: 2026-06-12
---

# TenantBasedWsFederationExternalLoginInfoProvider

Resolves WS-Federation metadata address, tenant and authority from settings for Active Directory Federation Services or Azure AD WS-Fed login.

## Public interface

- `override string Name { get; }`
- `override ExternalLoginProviderInfo GetExternalLoginInfo()`

## Dependencies

- [[tenant-based-external-login-info-provider-base]] abstract base providing cached credential resolution with tenant fallback logic

## Used by

- [[thinkn-insur-tech-web-host-module]]

## External dependencies

- Abp.AspNetZeroCore.Web
- Abp.Configuration
- Abp.Dependency

## Notes

Client secret is hardcoded as an empty string — WS-Federation does not use a client secret; the metadata address provides the public key.
