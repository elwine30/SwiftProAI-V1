---
title: "ThinknInsurTechWebHostModule"
type: module
language: csharp
layer: presentation
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Host/Startup/ThinknInsurTechWebHostModule.cs"
updated: 2026-06-12
---

# ThinknInsurTechWebHostModule

ABP module for the host project that wires up background workers, OCR service registration, external OAuth provider configuration and subscription expiry workers at startup.

## Public interface

- `override void PreInitialize()`
- `override void Initialize()`
- `override void PostInitialize()`

## Dependencies

- [[tenant-based-facebook-external-login-info-provider]] registers Facebook OAuth credentials per tenant
- [[tenant-based-google-external-login-info-provider]] registers Google OAuth credentials per tenant
- [[tenant-based-twitter-external-login-info-provider]] registers Twitter OAuth credentials per tenant
- [[tenant-based-microsoft-external-login-info-provider]] registers Microsoft OAuth credentials per tenant
- [[tenant-based-open-id-connect-external-login-info-provider]] registers OIDC credentials per tenant
- [[tenant-based-ws-federation-external-login-info-provider]] registers WS-Federation credentials per tenant

## Used by

- [[startup]]

## External dependencies

- Abp.AspNetZeroCore
- Abp.Modules
- Abp.Threading.BackgroundWorkers

## Notes

Conditionally registers IOCRPromptService as either the real OCRPromptService or MockOCRPromptService depending on the OCR:IsOCREnabled appsetting. Subscription workers are only started when multi-tenancy is enabled.
