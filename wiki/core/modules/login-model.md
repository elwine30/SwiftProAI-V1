---
title: "LoginModel"
type: class
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Host/Models/Ui/LoginModel.cs"
updated: 2026-06-12
---

# LoginModel

Form model for the server-side Razor login page collecting username, password, remember-me flag and optional tenancy name.

## Public interface

- `string UserNameOrEmailAddress { get; set; }`
- `string Password { get; set; }`
- `bool RememberMe { get; set; }`
- `string TenancyName { get; set; }`

## Used by

- [[ui-controller]]

## External dependencies

- Abp.Auditing

## Notes

Password is decorated with [DisableAuditing] to prevent it being written to audit logs.
