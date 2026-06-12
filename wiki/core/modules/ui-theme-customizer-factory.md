---
title: "UiThemeCustomizerFactory"
type: service
language: csharp
layer: infrastructure
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/UiCustomization/UiThemeCustomizerFactory.cs"
updated: 2026-06-12
---

# UiThemeCustomizerFactory

Resolves the correct Metronic theme customiser implementation from DI based on the currently active UI theme setting, defaulting to ThemeDefaultUiCustomizer for unknown theme names.

## Public interface

Task<IUiCustomizer> GetCurrentUiCustomizer()
IUiCustomizer GetUiCustomizer(string theme)

## External dependencies

- Abp.Configuration

## Notes

Supports 13 numbered Metronic themes plus a default. Theme names are compared case-insensitively.
