---
title: "HomePageModel"
type: class
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Host/Models/Ui/HomePageModel.cs"
updated: 2026-06-12
---

# HomePageModel

View model for the Razor home page carrying current login information and multi-tenancy state, with a helper to render the tenancy-prefixed username.

## Public interface

- `bool IsMultiTenancyEnabled { get; set; }`
- `GetCurrentLoginInformationsOutput LoginInformation { get; set; }`
- `string GetShownLoginName()`

## Used by

- [[ui-controller]]
