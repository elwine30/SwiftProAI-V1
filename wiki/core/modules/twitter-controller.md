---
title: "TwitterController"
type: controller
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Host/Controllers/TwitterController.cs"
updated: 2026-06-12
---

# TwitterController

Handles OAuth 1.0a Twitter login flow by obtaining request tokens and exchanging verifier codes for access tokens.

## Public interface

- `[HttpPost] async Task<TwitterGetRequestTokenResponse> GetRequestToken()`
- `[HttpPost] async Task<TwitterGetAccessTokenResponse> GetAccessToken(string token, string verifier)`

## External dependencies

- Abp.AspNetZeroCore.Web

## Notes

Route is api/Twitter/[action]. Delegates actual OAuth calls to TwitterAuthProviderApi from ABP Zero.
