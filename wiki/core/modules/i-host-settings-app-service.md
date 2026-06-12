---
title: "IHostSettingsAppService"
type: interface
language: csharp
layer: application
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Application.Shared/Configuration/Host/IHostSettingsAppService.cs"
updated: 2026-06-12
---

# IHostSettingsAppService

Contract for reading and updating all host-level platform settings and sending a test email to verify SMTP configuration.

## Public interface

Task<HostSettingsEditDto> GetAllSettings()
Task UpdateAllSettings(HostSettingsEditDto input)
Task SendTestEmail(SendTestEmailInput input)

## External dependencies

- Abp.Core
