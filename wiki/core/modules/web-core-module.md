---
title: "ThinknInsurTechWebCoreModule"
type: module
language: csharp
layer: infrastructure
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/ThinknInsurTechWebCoreModule.cs"
updated: 2026-06-12
---

# ThinknInsurTechWebCoreModule

ABP module root for the web infrastructure layer; wires JWT configuration, Hangfire, Redis cache, HTML sanitiser, and registers all assembly types with the IoC container.

## Public interface

override void PreInitialize()
override void Initialize()
override void PostInitialize()
private void ConfigureTokenAuth()
private void SetAppFolders()

## Dependencies

- [[token-auth-configuration]] populated with JWT signing key and expiration settings during PreInitialize
- [[app-configuration-accessor]] used to read appsettings for JWT and folder configuration
- [[app-configuration-writer]] writes resolved configuration values back for downstream use
- [[web-consts]] Hangfire dashboard enabled flag is set here based on config
- [[two-factor-code-cache-extensions]] cache registered for two-factor code storage
- [[signal-r-chat-communicator]] SignalR hub and communicator wired during initialisation

## Used by

- [[startup]] composed into the ASP.NET Core host startup pipeline

## External dependencies

- Abp.AspNetCore
- Abp.AspNetCore.SignalR
- Abp.AspNetZeroCore.Web
- Abp.Hangfire.AspNetCore
- Abp.RedisCache
- Abp.HtmlSanitizer

## Notes

Redis cache is commented out by default. Hangfire dashboard is disabled by default (WebConsts.HangfireDashboardEnabled = false). Depends on ThinknInsurTechGraphQLModule which is not in this project area.
