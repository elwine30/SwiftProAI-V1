---
title: "ThinknInsurTechCoreModule"
type: module
language: csharp
layer: domain
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Core/ThinknInsurTechCoreModule.cs"
updated: 2026-06-12
---

# ThinknInsurTechCoreModule

The root ABP module for the domain layer; wires together all providers, replaces default services and starts background workers.

## Public interface

- `PreInitialize()` — registers feature, setting, notification and webhook providers; configures multi-tenancy, caching, LDAP, MailKit and role management
- `Initialize()` — convention-registers all types in the assembly via the IoC container
- `PostInitialize()` — registers fallback `NullChatCommunicator`, `UserDelegationConfiguration`, starts `ChatUserStateWatcher` and records startup time

## Dependencies

- [[thinkninsurtech-domain-service-base]] base service used by domain services registered here
- [[app-feature-provider]] feature definitions
- [[app-setting-provider]] application settings
- [[app-notification-provider]] notification definitions
- [[app-webhook-definition-provider]] webhook definitions
- [[friendship-cache-item]] configures 30-minute sliding cache
- [[dashboard-configuration]] registered as IoC singleton
- [[ou-abp-session]] replaces default `IAbpSession`
- [[thinkninsurtech-smtp-email-sender-configuration]] replaces default SMTP config
- [[thinkninsurtech-mail-kit-smtp-builder]] replaces MailKit SMTP builder

## External dependencies

- Abp.Zero
- Abp.AspNetZeroCore
- Abp.AutoMapper
- Abp.MailKit
- Abp.Zero.Ldap
- Castle.Windsor (IoC container)

## Notes

- EF Core workaround switch `Microsoft.EntityFrameworkCore.Issue9825` is set in `PreInitialize()`.
- Twilio SMS and LDAP are commented out — enable via config by uncommenting the relevant `ReplaceService` / `Enable` calls.
- Email sending is suppressed (`NullEmailSender`) when `DebugHelper.IsDebug` is true.
