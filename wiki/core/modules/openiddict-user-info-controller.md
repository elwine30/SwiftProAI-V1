---
title: "UserInfoController"
type: controller
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/OpenIddict/Controllers/UserInfoController.cs"
updated: 2026-06-12
---

# UserInfoController

OpenIddict userinfo endpoint at connect/userinfo that returns scope-filtered claims (profile, email, phone, roles, tenant ID) for the authenticated user.

## Public interface

virtual Task<IActionResult> Userinfo()
protected virtual Task<Dictionary<string, object>> GetUserInfoClaims()

## Dependencies

- [[abp-open-id-dict-controller-base]] base class providing shared OpenIddict managers and helpers

## External dependencies

- OpenIddict.Abstractions
- OpenIddict.Server.AspNetCore
