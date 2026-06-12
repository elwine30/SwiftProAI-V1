---
title: "UiThemeCustomizerBase"
type: class
language: csharp
layer: infrastructure
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/UiCustomization/Metronic/UiThemeCustomizerBase.cs"
updated: 2026-06-12
---

# UiThemeCustomizerBase

Base class for Metronic theme customisers providing scoped setting read and write helpers that prefix all setting names with the theme name for namespaced storage.

## Public interface

Task<string> GetSettingValueAsync(string settingName)
Task<T> GetSettingValueAsync<T>(string settingName)
Task<string> GetSettingValueForApplicationAsync(string settingName)
Task<string> GetSettingValueForTenantAsync(string settingName, int tenantId)
virtual Task UpdateDarkModeSettingsAsync(UserIdentifier user, bool isDarkModeEnabled)
virtual Task<string> GetBodyClass()
virtual Task<string> GetBodyStyle()

## External dependencies

- Abp.Configuration
